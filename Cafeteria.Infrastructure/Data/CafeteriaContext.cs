using Cafeteria.Application.Pedidos;
using Cafeteria.Domain;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Cafeteria.Infrastructure.Data
{
  public class CafeteriaContext : DbContext, IUnitOfWork
  {
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<ItemPedido> ItensPedido { get; set; }

    public CafeteriaContext(DbContextOptions<CafeteriaContext> options) : base(options) { }

    public async Task<bool> CommitAsync()
    {
      // Salva as alterações no banco de dados SQLite
      return await SaveChangesAsync() > 0;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      // Mapeamentos básicos para garantir que o EF Core entenda a lista privada de itens no Pedido
      modelBuilder.Entity<Pedido>(entity =>
      {
        entity.HasKey(p => p.Id);

        // Mapeia a propriedade de navegação _itens mapeada na entidade
        entity.HasMany(p => p.Itens)
                    .WithOne()
                    .OnDelete(DeleteBehavior.Cascade);
      });

      modelBuilder.Entity<ItemPedido>(entity =>
      {
        entity.HasKey(i => i.Id);
        entity.Property(i => i.Nome).IsRequired().HasMaxLength(100);
        entity.Property(i => i.PrecoUnitario).HasColumnType("decimal(18,2)");
      });
    }
  }
}