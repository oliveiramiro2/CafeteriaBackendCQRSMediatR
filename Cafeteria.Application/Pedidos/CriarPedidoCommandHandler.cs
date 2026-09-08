using Cafeteria.Domain;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Cafeteria.Application.Pedidos
{
  public class CriarPedidoCommandHandler : IRequestHandler<CriarPedidoCommand, int>
  {
    private readonly IPedidoRepository _pedidoRepository;

    // O Handler recebe um repositório via Injeção de Dependência
    public CriarPedidoCommandHandler(IPedidoRepository pedidoRepository)
    {
      _pedidoRepository = pedidoRepository;
    }

    public async Task<int> Handle(CriarPedidoCommand request, CancellationToken cancellationToken)
    {
      // 1. Converte os DTOs da API em objetos de domínio (ItemPedido)
      var itensDominio = request.Itens.Select(i =>
          new ItemPedido(i.Nome, i.Quantidade, i.PrecoUnitario)
      ).ToList();

      // 2. Cria o agregado Pedido (aplicando as regras de negócio, como desconto)
      var pedido = new Pedido(itensDominio);

      // 3. Salva no banco através do repositório
      await _pedidoRepository.AdicionarAsync(pedido);
      await _pedidoRepository.UnitOfWork.CommitAsync();

      // 4. Retorna o ID do pedido gerado
      return pedido.Id;
    }
  }
}