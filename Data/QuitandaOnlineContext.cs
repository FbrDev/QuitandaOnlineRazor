using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuitandaOnline.Models;

namespace QuitandaOnline.Data
{
    public class QuitandaOnlineContext : IdentityDbContext<AppUser>
    {
        public QuitandaOnlineContext(DbContextOptions<QuitandaOnlineContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ItemPedido>()
                .HasKey(e => new { e.IdPedido, e.IdProduto });

            modelBuilder.Entity<Favorito>()
                .HasKey(e => new { e.IdCliente, e.IdProduto });

            modelBuilder.Entity<Visitado>()
                .HasKey(e => new { e.IdCliente, e.IdProduto });

            //restringe a exclusão de clientes que possuem pedidos
            modelBuilder.Entity<Pedido>()
                .HasOne<Cliente>(p => p.Cliente)
                .WithMany(c => c.Pedidos)
                .HasForeignKey(p => p.IdCliente)
                .OnDelete(DeleteBehavior.Restrict);

            //exclui automaticamento os itens de um pedido quando um pedido é excluído
            modelBuilder.Entity<ItemPedido>()
                .HasOne<Pedido>(ip => ip.Pedido)
                .WithMany(p => p.ItensPedido)
                .HasForeignKey(p => p.IdPedido)
                .OnDelete(DeleteBehavior.Cascade);

            //restringe exclusão de produtos que possuem itens pedidos
            modelBuilder.Entity<ItemPedido>()
                .HasOne<Produto>(ip => ip.Produto)
                .WithMany()
                .HasForeignKey(p => p.IdProduto)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ItemPedido> ItensPedido { get; set; }
        public DbSet<Favorito> Favoritos { get; set; }
        public DbSet<Visitado> Visitados { get; set; }
    }
}
