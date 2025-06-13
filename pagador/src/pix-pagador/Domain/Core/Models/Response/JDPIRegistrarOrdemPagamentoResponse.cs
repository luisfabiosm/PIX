using Domain.Core.Base;
using System.Text.Json;

namespace Domain.Core.Models.Response
{
    public record JDPIRegistrarOrdemPagamentoResponse : BaseTransactionResponse
    {
        public double valorCheqEspUtilizado { get; set; }
        public string agendamentoID { get; set; }
        public string comprovante { get; set; }


        public JDPIRegistrarOrdemPagamentoResponse(string result)
        {
            var _result = JsonSerializer.Deserialize<JDPIRegistrarOrdemPagamentoResponse>(result);

            valorCheqEspUtilizado = _result.valorCheqEspUtilizado;
            agendamentoID = _result.agendamentoID;
            comprovante = _result.comprovante;
            chvAutorizador = _result.chvAutorizador;

        }
    }
}
