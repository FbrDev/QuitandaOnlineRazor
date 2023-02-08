using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AspNetCoreWebApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetCoreWebApp.Pages.ProdutoCRUD
{
    public class IndexModel : PageModel
    {
        private readonly Data.ApplicationDbContext _context;

        public IndexModel(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Produto> Produto { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Produto != null)
            {
                Produto = await _context.Produto.ToListAsync();
            }
        }
    }
}
