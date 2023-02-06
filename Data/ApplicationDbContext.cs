using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AspNetCoreWebApp.Models;

namespace AspNetCoreWebApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Produto> Produto { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
    }
}
