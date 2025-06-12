using Domain.Core.Base;
using System.Text.Json;

namespace Domain.Core.Models.Responses
{
    public record JDPIEfetivarOrdemPagamentoResponse : BaseTransactionResponse
    {

        public JDPIEfetivarOrdemPagamentoResponse()
        {
            
        }

        public JDPIEfetivarOrdemPagamentoResponse(string result)
        {
            var _result = JsonSerializer.Deserialize<JDPIEfetivarOrdemPagamentoResponse>(result);

            chvAutorizador = _result.chvAutorizador;

        }
    }
}
