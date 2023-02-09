using AspNetCoreWebApp.Data;
using AspNetCoreWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreWebApp.Pages.ClienteCRUD
{
    public class AlterarModel : PageModel
    {
        private readonly QuitandaOnlineContext _context;

        [BindProperty]
        public Cliente Cliente { get; set; }

        public AlterarModel(QuitandaOnlineContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            Cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.IdCliente == id);

            if(Cliente == null) return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid) return NotFound();

            _context.Attach(Cliente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }catch (DbUpdateConcurrencyException)
            {
                if (!ClienteAindaExiste(Cliente.IdCliente))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Listar");
        }

        private bool ClienteAindaExiste(int idCliente)
        {
            return _context.Clientes.Any(m => m.IdCliente == idCliente);
        }
    }
}
