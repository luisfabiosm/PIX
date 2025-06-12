using Domain.Core.Base;
using Domain.Core.Models.Responses;
using System.Text.Json;

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
            return JsonSerializer.Serialize(this);
        }
    }
}
