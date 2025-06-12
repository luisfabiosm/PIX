using Domain.Core.Base;
using System.Text.Json;

namespace Domain.Core.Models.Responses
{
    public record JDPICreditoOrdemPagamentoResponse: BaseTransactionResponse
    {
        public string idReqJdPi { get; set; }

        public string idCreditoSgct { get; set; }

        public string dtHrCreditoSgct { get; set; }

        public JDPICreditoOrdemPagamentoResponse()
        {
            
        }


        public JDPICreditoOrdemPagamentoResponse(string result)
        {
            var _result = JsonSerializer.Deserialize<JDPICreditoOrdemPagamentoResponse>(result);

            idReqJdPi = _result.idReqJdPi;
            idCreditoSgct = _result.idCreditoSgct;
            dtHrCreditoSgct = _result.dtHrCreditoSgct;

        }
    }
}
