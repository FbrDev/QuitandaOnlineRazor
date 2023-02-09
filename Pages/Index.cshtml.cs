using AspNetCoreWebApp.Data;
using AspNetCoreWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetCoreWebApp.Pages
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

        public async Task OnGet()
        {
            Produtos = await _context.Produtos.ToListAsync<Produto>();
        }
    }
}