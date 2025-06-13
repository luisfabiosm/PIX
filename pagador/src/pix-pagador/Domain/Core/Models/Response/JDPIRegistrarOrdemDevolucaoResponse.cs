using Domain.Core.Base;
using System.Text.Json;

namespace Domain.Core.Models.Response
{
    public record JDPIRegistrarOrdemDevolucaoResponse : BaseTransactionResponse
    {
        public bool pixInterno { get; set; }

        public JDPIRegistrarOrdemDevolucaoResponse(string result)
        {
            var _result = JsonSerializer.Deserialize<JDPIRegistrarOrdemDevolucaoResponse>(result);

            pixInterno = _result.pixInterno;


        }
    }
}
