using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using QuitandaOnline.Data;
using QuitandaOnline.Models;
using QuitandaOnline.Services;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace QuitandaOnline.Pages
{
    public class NovoClienteModel : PageModel
    {
        public class Senhas
        {
            [Required(ErrorMessage = "O campo \"{0}\" � de preenchimento obrigat�rio.")]
            [StringLength(16, ErrorMessage = "O campo \"{0}\" deve ter pelo menos {2} e no m�ximo {1} caracteres", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Senha")]
            public string Senha { get; set; }

            [Required(ErrorMessage = "O campo \"{0}\" � de preenchimento obrigat�rio.")]
            [DataType(DataType.Password)]
            [Display(Name = "Confirma��o de Senha")]
            [Compare("Senha", ErrorMessage = "A confirma��o da senha n�o confere com a senha informada.")]
            public string ConfirmacaoSenha { get; set; }
        }


        private readonly QuitandaOnlineContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;

        public NovoClienteModel(QuitandaOnlineContext context,
                                UserManager<AppUser> userManager, 
                                RoleManager<IdentityRole> roleManager,
                                IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public Cliente Cliente { get; set; }

        [BindProperty]
        public Senhas SenhasUsuario { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //cria um novo objeto Cliente
            var cliente = new Cliente();
            cliente.Endereco = new Endereco();

            //novos clientes sempre iniciam com essa situa��o
            cliente.Situacao = Cliente.SituacaoCliente.Cadastrado;

            var senhasUsuario = new Senhas();
            if (!await TryUpdateModelAsync(senhasUsuario, senhasUsuario.GetType(), nameof(senhasUsuario)))
                return Page();

            //tenta atualizar o novo objeto com os dados oriundos do formul�rio
            if (await TryUpdateModelAsync(cliente, Cliente.GetType(), nameof(Cliente)))
            {
                //verifica se o perfil de usu�rio "cliente" existe
                if (!await _roleManager.RoleExistsAsync("cliente"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("cliente"));
                }

                //verifica se j� existe um usu�rio com o e-mail informado
                var usuarioExistente = await _userManager.FindByEmailAsync(cliente.Email);
                if (usuarioExistente != null)
                {
                    //adiciona um erro na propriedade Email do cliente para que o campo apresente o erro no formul�rio
                    ModelState.AddModelError("Cliente.Email", "J� existe um cliente cadastrado com este e-mail.");
                    return Page();
                }

                //cria o objeto usu�rio Identity e adiciona ao perfil "cliente"
                var usuario = new AppUser()
                {
                    UserName = cliente.Email,
                    Email = cliente.Email,
                    PhoneNumber = cliente.Telefone,
                    Nome = cliente.Nome
                };

                //cria usu�rio no banco de dados
                var result = await _userManager.CreateAsync(usuario, senhasUsuario.Senha);

                //se a cria��o do usu�rio Identity foi bem sucedida
                if (result.Succeeded)
                {
                    //associa o usu�rio ao perfil "cliente"
                    await _userManager.AddToRoleAsync(usuario, "cliente");

                    //adiciona o novo objeto cliente ao contexto de banco de dados atual e salva no banco de dados
                    _context.Clientes.Add(cliente);
                    int afetados = await _context.SaveChangesAsync();
                    //se salvou o cliente no banco de dados
                    if (afetados > 0)
                    {
                        //envia uma mensagem ao usu�rio para que ele confirme seu e-mail	
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(usuario);
                        token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                        var urlConfirmacaoEmail = Url.Page("/ConfirmacaoEmail", null,
                            new { userId = usuario.Id, token }, Request.Scheme);
                        StringBuilder msg = new StringBuilder();
                        msg.Append("<h1>Quitanda Online :: Confirma��o de E-mail</h1>");
                        msg.Append($"<p>Por favor, confirme seu e-mail " +
                            $"<a href='{HtmlEncoder.Default.Encode(urlConfirmacaoEmail)}'>clicando aqui</a>.</p>");
                        msg.Append("<p>Atenciosamente,<br>Equipe de Suporte Quitanda Online</p>");
                        await _emailSender.SendEmailAsync(usuario.Email, "Confirma��o de E-mail", "", msg.ToString());

                        return RedirectToPage("/CadastroRealizado");
                    }
                    else
                    {
                        //exclui o usu�rio Identity criado
                        await _userManager.DeleteAsync(usuario);
                        ModelState.AddModelError("Cliente", "N�o foi poss�vel efetuar o cadastro. Verifique os dados " +
                            "e tente novamente. Se o problema persistir, entre em contato conosco.");
                        return Page();
                    }
                }
                else
                {
                    ModelState.AddModelError("Cliente.Email", "N�o foi poss�vel criar um usu�rio com este endere�o de e-mail. " +
                        "Use outro endere�o de e-mail ou tente recuperar a senha deste.");
                }
            }

            return Page();
        }
    }
}
