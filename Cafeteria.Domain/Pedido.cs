using System;
using System.Collections.Generic;
using System.Linq;

namespace Cafeteria.Domain
{
  public class Pedido
  {
    private readonly List<ItemPedido> _itens = new List<ItemPedido>();

    public int Id { get; private set; }
    public IReadOnlyCollection<ItemPedido> Itens => _itens.AsReadOnly();
    public decimal ValorTotal { get; private set; }
    public StatusPedido Status { get; private set; }

    // Construtor privado exigido pelo EF Core
    private Pedido() { }

    public Pedido(List<ItemPedido> itens)
    {
      if (itens == null || !itens.Any())
      {
        throw new ArgumentException("O pedido precisa ter pelo menos um item.");
      }

      _itens = itens;
      Status = StatusPedido.Pendente;
      CalcularValorTotal();
    }

    public void AdicionarItem(ItemPedido item)
    {
      _itens.Add(item);
      CalcularValorTotal();
    }

    public void Pagar()
    {
      if (Status == StatusPedido.Cancelado)
        throw new InvalidOperationException("Não é possível pagar um pedido que foi cancelado.");

      if (Status == StatusPedido.Pago)
        throw new InvalidOperationException("Este pedido já está pago.");

      Status = StatusPedido.Pago;
    }

    public void Cancelar()
    {
      if (Status == StatusPedido.Pago)
        throw new InvalidOperationException("Não é possível cancelar um pedido que já foi pago.");

      if (Status == StatusPedido.Cancelado)
        throw new InvalidOperationException("Este pedido já está cancelado.");

      Status = StatusPedido.Cancelado;
    }

    private void CalcularValorTotal()
    {
      decimal bruto = _itens.Sum(i => i.CalcularSubtotal());

      // Regra de negócio de desconto mantida no domínio!
      if (bruto > 100)
      {
        ValorTotal = bruto * 0.90m; // 10% de desconto
      }
      else
      {
        ValorTotal = bruto;
      }
    }
  }
}