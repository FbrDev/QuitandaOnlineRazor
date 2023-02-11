using QuitandaOnline.Data;
using QuitandaOnline.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QuitandaOnline.Pages.ProdutoCRUD
{
    public class AlterarModel : PageModel
    {
        private readonly QuitandaOnlineContext _context;

        [BindProperty]
        public Produto Produto { get; set; }

        public AlterarModel(QuitandaOnlineContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            Produto = await _context.Produtos.FirstOrDefaultAsync(c => c.IdProduto == id);

            if(Produto == null) return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid) return NotFound();

            _context.Attach(Produto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoAindaExiste(Produto.IdProduto))
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

        private bool ProdutoAindaExiste(int idProduto)
        {
            return _context.Produtos.Any(m => m.IdProduto == idProduto);
        }
    }
}
