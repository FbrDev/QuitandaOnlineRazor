using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuitandaOnline.Models;

namespace QuitandaOnline.Data
{
    public class QuitandaOnlineContext : DbContext
    {
        public QuitandaOnlineContext(DbContextOptions<QuitandaOnlineContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemPedido>()
                .HasKey(e => new { e.IdPedido, e.IdProduto });

            modelBuilder.Entity<Favorito>()
                .HasKey(e => new { e.IdCliente, e.IdProduto });

            modelBuilder.Entity<Visitado>()
                .HasKey(e => new { e.IdCliente, e.IdProduto });
        }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ItemPedido> ItensPedido { get; set; }
        public DbSet<Favorito> Favoritos { get; set; }
        public DbSet<Visitado> Visitados { get; set; }
    }
}
