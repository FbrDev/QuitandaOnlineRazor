using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AspNetCoreWebApp.Data;
using AspNetCoreWebApp.Models;

namespace AspNetCoreWebApp.Pages.ProdutoCRUD
{
    public class DetailsModel : PageModel
    {
        private readonly AspNetCoreWebApp.Data.QuitandaOnlineContext _context;

        public DetailsModel(AspNetCoreWebApp.Data.QuitandaOnlineContext context)
        {
            _context = context;
        }

      public Produto Produto { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Produtos == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos.FirstOrDefaultAsync(m => m.IdProduto == id);
            if (produto == null)
            {
                return NotFound();
            }
            else 
            {
                Produto = produto;
            }
            return Page();
        }
    }
}
