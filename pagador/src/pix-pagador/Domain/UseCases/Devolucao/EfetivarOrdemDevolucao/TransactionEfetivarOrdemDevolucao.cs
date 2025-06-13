using Domain.Core.Base;
using Domain.Core.Models.Response;
using System.Text.Json;

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
            return JsonSerializer.Serialize(this);
        }
    }
}
