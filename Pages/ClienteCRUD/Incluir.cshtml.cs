using QuitandaOnline.Data;
using QuitandaOnline.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace QuitandaOnline.Pages.ClienteCRUD
{
    [Authorize(Policy = "isAdmin")]
    public class IncluirModel : PageModel
    {
        private readonly QuitandaOnlineContext _context;

        [BindProperty]
        public Cliente Cliente { get; set; }

        public IncluirModel(QuitandaOnlineContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var cliente = new Cliente();
            cliente.Endereco= new Endereco();
            cliente.Situacao = Cliente.SituacaoCliente.Cadastrado;

            if(await TryUpdateModelAsync(cliente, Cliente.GetType(), nameof(Cliente)))
            {
                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Listar");
            }
            return Page();
        }
    }
}
