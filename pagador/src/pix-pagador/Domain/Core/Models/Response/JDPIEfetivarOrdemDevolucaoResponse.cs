using Domain.Core.Base;
using System.Text.Json;

namespace Domain.Core.Models.Response
{
    public record JDPIEfetivarOrdemDevolucaoResponse : BaseTransactionResponse
    {

        public JDPIEfetivarOrdemDevolucaoResponse()
        {
            
        }
      

        public JDPIEfetivarOrdemDevolucaoResponse(string result)
        {
            var _result = JsonSerializer.Deserialize<JDPIEfetivarOrdemDevolucaoResponse>(result);

            chvAutorizador = _result.chvAutorizador;

        }
    }
}
