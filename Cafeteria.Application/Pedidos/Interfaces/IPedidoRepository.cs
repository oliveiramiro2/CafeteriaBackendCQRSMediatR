using Cafeteria.Domain;
using System.Threading.Tasks;

namespace Cafeteria.Application.Pedidos
{
  public interface IUnitOfWork
  {
    Task<bool> CommitAsync();
  }

  public interface IPedidoRepository
  {
    Task AdicionarAsync(Pedido pedido);
    Task<Pedido> ObterPorIdAsync(int id);
    IUnitOfWork UnitOfWork { get; }
  }
}