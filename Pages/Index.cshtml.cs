using QuitandaOnline.Data;
using QuitandaOnline.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace QuitandaOnline.Pages
{
    public class IndexModel : PageModel
    {
        private const int tamanhoPagina = 12;
        private readonly ILogger<IndexModel> _logger;
        private readonly QuitandaOnlineContext _context;

        public int PaginaAtual { get; set; }
        public int QuantidadePaginas { get; set; }


        public IList<Produto> Produtos;

        public IndexModel(ILogger<IndexModel> logger, QuitandaOnlineContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task OnGet([FromQuery(Name = "q")]string termoBusca, [FromQuery(Name = "o")] int? ordem = 1, [FromQuery(Name = "p")]int? pagina = 1)
        {
            this.PaginaAtual = pagina.Value;

            var query = _context.Produtos.AsQueryable();

            if (!string.IsNullOrEmpty(termoBusca))
            {
                query = query.Where(p => p.Nome.ToLower().Contains(termoBusca.ToLower()));
            }

            if (ordem.HasValue)
            {
                switch (ordem.Value)
                {
                    case 1:
                        query = query.OrderBy(p => p.Nome.ToLower());
                        break;
                    case 2:
                        query = query.OrderBy(p => p.Preco);
                        break;
                    case 3:
                        query = query.OrderByDescending(p => p.Nome);
                        break;
                }
            }

            var queryCount = query;
            int quantidadeProduto = queryCount.Count();
            this.QuantidadePaginas = Convert.ToInt32(Math.Ceiling(quantidadeProduto * 1M / tamanhoPagina));
            
            query = query.Skip(tamanhoPagina * (this.PaginaAtual - 1)).Take(tamanhoPagina);

            Produtos = await query.ToListAsync();
        }
    }
}