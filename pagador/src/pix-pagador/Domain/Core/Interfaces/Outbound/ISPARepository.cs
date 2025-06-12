

using Domain.UseCases.Devolucao.CancelarOrdemDevolucao;
using Domain.UseCases.Devolucao.EfetivarOrdemDevolucao;
using Domain.UseCases.Devolucao.RegistrarOrdemDevolucao;
using Domain.UseCases.Pagamento.CancelarOrdemPagamento;
using Domain.UseCases.Pagamento.EfetivarOrdemPagamento;
using Domain.UseCases.Pagamento.RegistrarOrdemPagamento;

namespace Domain.Core.Interfaces.Outbound
{
    public interface ISPARepository : IDisposable
    {

  
        ValueTask<(string result, Exception exception)> RegistrarOrdemPagamento(TransactionRegistrarOrdemPagamento transaction);

        ValueTask<(string result, Exception exception)> EfetivarOrdemPagamento(TransactionEfetivarOrdemPagamento transaction);

        ValueTask<(string result, Exception exception)> CancelarOrdemPagamento(TransactionCancelarOrdemPagamento transaction);

        ValueTask<(string result, Exception exception)> RegistrarOrdemDevolucao(TransactionRegistrarOrdemDevolucao transaction);

        ValueTask<(string result, Exception exception)> EfetivarOrdemDevolucao(TransactionEfetivarOrdemDevolucao transaction);

        ValueTask<(string result, Exception exception)> CancelarOrdemDevolucao(TransactionCancelarOrdemDevolucao transaction);

    }
}
