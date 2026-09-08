using MediatR;
using System.Collections.Generic;

namespace Cafeteria.Application.Pedidos
{
  public class CriarPedidoCommand : IRequest<int>
  {
    public List<CriarItemPedidoDto> Itens { get; set; } = new();
  }
}