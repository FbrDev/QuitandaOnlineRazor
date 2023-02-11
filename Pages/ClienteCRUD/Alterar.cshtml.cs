using QuitandaOnline.Data;
using QuitandaOnline.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QuitandaOnline.Pages.ClienteCRUD
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
            //para garantir que o CPF e o e-mail não serão atualizados
            var cliente = await _context.Clientes.Select(m => new { m.IdCliente, m.Email, m.CPF }).FirstOrDefaultAsync();
            Cliente.Email = cliente.Email;
            Cliente.CPF = cliente.CPF;

            //ModelState.ClearValidationState("Cliente.Email");
            //ModelState.ClearValidationState("Cliente.CPF");

            if (ModelState.Keys.Contains("Cliente.Email"))
            {
                ModelState["Cliente.Email"].Errors.Clear();
                ModelState.Remove("Cliente.Email");
            }
            if (ModelState.Keys.Contains("Cliente.CPF"))
            {
                ModelState["Cliente.CPF"].Errors.Clear();
                ModelState.Remove("Cliente.CPF");
            }

            if(!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Cliente).State = EntityState.Modified;
            _context.Attach(Cliente.Endereco).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
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
