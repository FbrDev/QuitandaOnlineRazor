using QuitandaOnline.Data;
using QuitandaOnline.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace QuitandaOnline.Pages.ProdutoCRUD
{
    public class ListarModel : PageModel
    {
        private readonly QuitandaOnlineContext _context;

        public ListarModel(QuitandaOnlineContext context)
        {
            _context = context;
        }

        public IList<Produto> Produtos { get; set; }

        public async Task OnGetAsync()
        {
            Produtos = await _context.Produtos.OrderBy(p => p.Nome).ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos.FindAsync(id);

            if (produto != null) {
                _context.Produtos.Remove(produto);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Listar");
        }
    }
}
