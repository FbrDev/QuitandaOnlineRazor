using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using QuitandaOnline.Models;

namespace QuitandaOnline.Pages
{
    public class RedefinirSenhaModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;

        public RedefinirSenhaModel(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public DadosRedefinicaoSenha Dados { get; set; }

        public class DadosRedefinicaoSenha
        {
            [Required(ErrorMessage = "O campo \"{0}\" é de preenchimento obrigatório.")]
            [EmailAddress]
            public string Email { get; set; }

            [Required(ErrorMessage = "O campo \"{0}\" é de preenchimento obrigatório.")]
            [StringLength(100, ErrorMessage = "O campo \"{0}\" deve ter no mínimo {2} e no máximo {1} caracteres.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Senha { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmação de Senha")]
            [Compare("Senha", ErrorMessage = "A senha e a confirmação de senha estão divergentes.")]
            public string ConfirmacaoSenha { get; set; }

            public string Token { get; set; }
        }

        public IActionResult OnGet(string token = null)
        {
            if (token == null)
            {
                return BadRequest("Um token deve ser fornecido para redefinir a senha.");
            }
            else
            {
                Dados = new DadosRedefinicaoSenha
                {
                    Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token))
                };
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var usuario = await _userManager.FindByEmailAsync(Dados.Email);
            if (usuario == null)
            {
                // Não revela que o usuário não existe
                return RedirectToPage("/ConfirmacaoRedefinicaoSenha");
            }

            var resultado = await _userManager.ResetPasswordAsync(usuario, Dados.Token, Dados.Senha);
            if (resultado.Succeeded)
            {
                return RedirectToPage("./ConfirmacaoRedefinicaoSenha");
            }

            foreach (var erro in resultado.Errors)
            {
                ModelState.AddModelError(string.Empty, erro.Description);
            }

            return Page();
        }
    }
}