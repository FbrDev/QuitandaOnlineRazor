using QuitandaOnline.Data;
using QuitandaOnline.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace QuitandaOnline.Pages.ProdutoCRUD
{
    public class IncluirModel : PageModel
    {
        private readonly QuitandaOnlineContext _context;

        [BindProperty]
        public Produto Produto { get; set; }

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
            var produto = new Produto();
            if(ModelState.IsValid)
            {
                _context.Produtos.Add(produto);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Listar");
            }
            return Page();
        }
    }
}
