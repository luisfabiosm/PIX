using Domain.Core.Common.ResultPattern;
using Domain.Core.Common.Serialization;
using Domain.Core.Common.Transaction;
using Domain.Core.Enum;
using Domain.Core.Models.Response;

namespace Domain.UseCases.Pagamento.CancelarOrdemPagamento
{
    public record TransactionCancelarOrdemPagamento : BaseTransaction<BaseReturn<JDPICancelarOrdemPagamentoResponse>>
    {


        public string idReqSistemaCliente { get; set; }
        public string agendamentoID { get; set; }
        public string motivo { get; set; }
        public EnumTipoErro tipoErro { get; set; }


        public TransactionCancelarOrdemPagamento()
        {

        }


        public override string getTransactionSerialization()
        {
            return this.ToJsonOptimized(JsonOptions.Minimal);

        }
    }
}
