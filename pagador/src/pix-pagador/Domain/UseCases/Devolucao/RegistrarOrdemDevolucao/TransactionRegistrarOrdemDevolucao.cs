using Domain.Core.Base;
using Domain.Core.Models.Responses;
using System.Text.Json;

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
            return JsonSerializer.Serialize(this);
        }
    }
}
