using Domain.Core.Base;
using System.Text.Json;

namespace Domain.Core.Models.Response
{
    public record JDPICancelarOrdemDevolucaoResponse: BaseTransactionResponse
    {
        public JDPICancelarOrdemDevolucaoResponse()
        {

        }

        public JDPICancelarOrdemDevolucaoResponse(string result)
        {
            var _result = JsonSerializer.Deserialize<JDPICancelarOrdemDevolucaoResponse>(result);

            chvAutorizador = _result.chvAutorizador;

        }
    }
}
