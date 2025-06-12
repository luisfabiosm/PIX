using Domain.Core.Base;
using System.Text.Json;

namespace Domain.Core.Models.Responses
{
    public record JDPICancelarOrdemPagamentoResponse : BaseTransactionResponse
    {
        public JDPICancelarOrdemPagamentoResponse()
        {
            
        }

        public JDPICancelarOrdemPagamentoResponse(string result)
        {
            var _result = JsonSerializer.Deserialize<JDPICancelarOrdemPagamentoResponse>(result);

            chvAutorizador = _result.chvAutorizador;

        }
    }
}
