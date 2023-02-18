using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using QuitandaOnline.Data;
using QuitandaOnline.Models;
using QuitandaOnline.Services;

namespace QuitandaOnline.Pages
{
    public class RecuperarSenhaModel : PageModel
    {
        private UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailSender;

        public class DadosEmail
        {
            [Required(ErrorMessage = "O campo \"{0}\" é de preenchimento obrigatório.")]
            [EmailAddress]
            [Display(Name = "E-mail")]
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }
        }

        [BindProperty]
        public DadosEmail Dados { get; set; }

        public RecuperarSenhaModel(QuitandaOnlineContext context,
            UserManager<AppUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                AppUser usuario = await _userManager.FindByNameAsync(Dados.Email);
                if (usuario != null)
                {
                    string token = await _userManager.GeneratePasswordResetTokenAsync(usuario);
                    token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                    var urlResetarSenha = Url.Page("/RedefinirSenha",
                        null, new { token }, Request.Scheme);

                    StringBuilder msg = new StringBuilder();
                    msg.Append("<h1>Quitanda Online :: Recuperação de Senha</h1>");
                    msg.Append($"<p>Por favor, redefina sua senha <a href='{HtmlEncoder.Default.Encode(urlResetarSenha)}'>clicando aqui</a>.</p>");
                    msg.Append("<p>Atenciosamente<br>Equipe de Suporte Quitanda Online</p>");
                    await _emailSender.SendEmailAsync(usuario.Email, "Recuperação de Senha", "", msg.ToString());
                    return RedirectToPage("/EmailRecuperacaoEnviado");
                }
                else
                {
                    //Não é seguro informar ao usuário que o e-mail informado 
                    return RedirectToPage("/EmailRecuperacaoEnviado");
                    //ModelState.AddModelError("Dados.Email", "Nenhum usuário foi encontrado com este e-mail. " +
                    //    "Confira e tente novamente.");                    
                }
            }

            return Page();
        }
    }
}