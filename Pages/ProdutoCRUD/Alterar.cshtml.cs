using QuitandaOnline.Data;
using QuitandaOnline.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace QuitandaOnline.Pages.ProdutoCRUD
{
    public class AlterarModel : PageModel
    {
        private readonly QuitandaOnlineContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        [BindProperty]
        public Produto Produto { get; set; }

        public string CaminhoImagem { get; set; }

        [BindProperty]
        [Display(Name = "Imagem do produto")]
        public IFormFile ImagemProduto { get; set; }

        public AlterarModel(QuitandaOnlineContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            Produto = await _context.Produtos.FirstOrDefaultAsync(c => c.IdProduto == id);

            if(Produto == null) return NotFound();

            CaminhoImagem = $"~/Img/Produto/{Produto.IdProduto:D6}.jpg";

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid) return NotFound();

            _context.Attach(Produto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                // se há uma imagem de produto submetida
                if (ImagemProduto != null)
                    await AppUtils.ProcessarArquivoDeImagem(Produto.IdProduto, ImagemProduto, _webHostEnvironment);
            }catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExiste(Produto.IdProduto))
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

        private bool ProdutoExiste(int idProduto)
        {
            return _context.Produtos.Any(m => m.IdProduto == idProduto);
        }
    }
}
