using Domain.Core.Common.ResultPattern;
using Domain.Core.Common.Serialization;
using Domain.Core.Common.Transaction;
using Domain.Core.Models.Response;

namespace Domain.UseCases.Devolucao.RegistrarOrdemDevolucao
{
    public record TransactionRegistrarOrdemDevolucao : BaseTransaction<BaseReturn<JDPIRegistrarOrdemDevolucaoResponse>>
    {
        public string idReqSistemaCliente { get; set; }

        public string endToEndIdOriginal { get; set; }

        public string endToEndIdDevolucao { get; set; }

        public string codigoDevolucao { get; set; }

        public string motivoDevolucao { get; set; }

        public double valorDevolucao { get; set; }

        public TransactionRegistrarOrdemDevolucao()
        {

        }

        public override string getTransactionSerialization()
        {
            return this.ToJsonOptimized(JsonOptions.Minimal);

        }
    }
}
