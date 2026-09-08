using Cafeteria.Application.Pedidos;
using Cafeteria.Domain;
using Cafeteria.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Cafeteria.Infrastructure.Repositories
{
  public class PedidoRepository : IPedidoRepository
  {
    private readonly CafeteriaContext _context;

    public PedidoRepository(CafeteriaContext context)
    {
      _context = context;
    }

    public IUnitOfWork UnitOfWork => _context;

    public async Task AdicionarAsync(Pedido pedido)
    {
      await _context.Pedidos.AddAsync(pedido);
    }

    public async Task<Pedido> ObterPorIdAsync(int id)
    {
      return await _context.Pedidos
          .Include(p => p.Itens)
          .FirstOrDefaultAsync(p => p.Id == id);
    }
  }
}