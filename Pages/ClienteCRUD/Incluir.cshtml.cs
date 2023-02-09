using AspNetCoreWebApp.Data;
using AspNetCoreWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace AspNetCoreWebApp.Pages.ClienteCRUD
{
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
            if(await TryUpdateModelAsync<Cliente>(cliente, "cliente", obj => obj.Nome, obj => obj.DataNascimento, obj => obj.Email, obj => obj.CPF))
            {
                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Listar");
            }
            return Page();
        }
    }
}
