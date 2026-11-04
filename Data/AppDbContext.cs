using Microsoft.EntityFrameworkCore;
using LojaGames.Models;

namespace LojaGames.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Jogo> Jogos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ItemPedido> ItensPedido { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configura o banco de dados SQLite local
            optionsBuilder.UseSqlite("Data Source=lojagames.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações adicionais de relacionamentos se necessário (Fluent API)
            modelBuilder.Entity<ItemPedido>()
                .HasOne(ip => ip.Pedido)
                .WithMany(p => p.ItensPedido)
                .HasForeignKey(ip => ip.PedidoId);

            modelBuilder.Entity<ItemPedido>()
                .HasOne(ip => ip.Jogo)
                .WithMany(j => j.ItensPedido)
                .HasForeignKey(ip => ip.JogoId);
        }
    }
}