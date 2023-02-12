using QuitandaOnline.Data;
using QuitandaOnline.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace QuitandaOnline.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly QuitandaOnlineContext _context;

        public IList<Produto> Produtos;

        public IndexModel(ILogger<IndexModel> logger, QuitandaOnlineContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task OnGet([FromQuery(Name = "q")]string termoBusca, [FromQuery(Name = "o")] int? ordem = 1)
        {
            var query = _context.Produtos.AsQueryable();

            if (!string.IsNullOrEmpty(termoBusca))
            {
                query = query.Where(p => p.Nome.ToUpper().Contains(termoBusca.ToUpper()));
            }

            if (ordem.HasValue)
            {
                switch (ordem.Value)
                {
                    case 1:
                        query = query.OrderBy(p => p.Nome);
                        break;
                    case 2:
                        query = query.OrderBy(p => p.Preco);
                        break;
                    case 3:
                        query = query.OrderByDescending(p => p.Nome);
                        break;
                }
            }

            Produtos = await query.ToListAsync();
        }
    }
}