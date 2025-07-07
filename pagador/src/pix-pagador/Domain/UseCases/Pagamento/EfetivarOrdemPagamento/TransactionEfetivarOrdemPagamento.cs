using Domain.Core.Common.ResultPattern;
using Domain.Core.Common.Serialization;
using Domain.Core.Common.Transaction;
using Domain.Core.Models.Response;

namespace Domain.UseCases.Pagamento.EfetivarOrdemPagamento
{
    public record TransactionEfetivarOrdemPagamento : BaseTransaction<BaseReturn<JDPIEfetivarOrdemPagamentoResponse>>
    {

        public string idReqSistemaCliente { get; set; }

        public string idReqJdPi { get; set; }

        public string endToEndId { get; set; }

        public string dtHrReqJdPi { get; set; }

        public string agendamentoID { get; set; }


        public TransactionEfetivarOrdemPagamento()
        {

        }
        public override string getTransactionSerialization()
        {
            return this.ToJsonOptimized(JsonOptions.Minimal);

        }
    }
}
