using Domain.Core.Base;
using Domain.Core.Models.Responses;
using System.Text.Json;

namespace Domain.UseCases.Devolucao.CancelarOrdemDevolucao
{
    public record TransactionCancelarOrdemDevolucao : BaseTransaction<BaseReturn<JDPICancelarOrdemDevolucaoResponse>>
    {

        public string idReqSistemaCliente { get; set; }

        public TransactionCancelarOrdemDevolucao()
        {

        }

        public override string getTransactionSerialization()
        {
            return JsonSerializer.Serialize(this);
        }

    }
}
