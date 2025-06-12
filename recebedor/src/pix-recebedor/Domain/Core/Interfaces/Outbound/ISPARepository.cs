


using Domain.UseCases.CreditoDevolucao;
using Domain.UseCases.CreditoOrdemPagamento;
using Domain.UseCases.ValidarContaCliente;

namespace Domain.Core.Interfaces.Outbound
{
    public interface ISPARepository : IDisposable
    {

  
        ValueTask<(string result, Exception exception)> ValidarConta(TransactionValidarContaCliente transaction);


        ValueTask<(string result, Exception exception)> RegistrarCreditoOrdemPagamento(TransactionCreditoOrdemPagamento transaction);


        ValueTask<(string result, Exception exception)> RegistrarCreditoDevolucao(TransactionCreditoDevolucao transaction);


    }
}
