using Domain.Core.Common.ResultPattern;
using Domain.Core.Common.Serialization;
using Domain.Core.Common.Transaction;
using Domain.Core.Models.Response;

namespace Domain.UseCases.Devolucao.EfetivarOrdemDevolucao
{
    public record TransactionEfetivarOrdemDevolucao : BaseTransaction<BaseReturn<JDPIEfetivarOrdemDevolucaoResponse>>
    {
        public string idReqSistemaCliente { get; set; }

        public string idReqJdPi { get; set; }

        public string endToEndIdOriginal { get; set; }

        public string endToEndIdDevolucao { get; set; }

        public string dtHrReqJdPi { get; set; }

        public TransactionEfetivarOrdemDevolucao()
        {

        }

        public override string getTransactionSerialization()
        {
            return this.ToJsonOptimized(JsonOptions.Minimal);

        }
    }
}
