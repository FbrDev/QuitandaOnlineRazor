using QuitandaOnline.Data;
using QuitandaOnline.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace QuitandaOnline.Pages.ProdutoCRUD
{
    public class IncluirModel : PageModel
    {
        private readonly QuitandaOnlineContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        [BindProperty]
        public Produto Produto { get; set; }

        public string CaminhoImagem { get; set; }

        [BindProperty]
        [Display(Name = "Imagem do Produto")]
        [Required(ErrorMessage = "O campo \"{0}\" é de preenchimento obrigatório.")]
        public IFormFile ImagemProduto { get; set; }

        public IncluirModel(QuitandaOnlineContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            CaminhoImagem = "~/Img/Produto/sem_imagem.jpg";
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ImagemProduto == null) return Page();

            var produto = new Produto();

            if(await TryUpdateModelAsync(produto, Produto.GetType(), nameof(Produto)))
            {
                _context.Produtos.Add(produto);
                await _context.SaveChangesAsync();
                await AppUtils.ProcessarArquivoDeImagem(produto.IdProduto, ImagemProduto, _webHostEnvironment);
                return RedirectToPage("./Listar");
            }
            return Page();
        }
    }
}
