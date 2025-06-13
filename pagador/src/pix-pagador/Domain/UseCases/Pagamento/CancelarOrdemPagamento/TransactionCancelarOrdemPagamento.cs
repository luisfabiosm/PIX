using Domain.Core.Base;
using Domain.Core.Enum;
using Domain.Core.Models.Response;
using System.Text.Json;

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
            return JsonSerializer.Serialize(this);
        }
    }
}
