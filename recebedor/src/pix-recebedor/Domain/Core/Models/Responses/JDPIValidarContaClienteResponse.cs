using Domain.Core.Base;
using System.Text.Json;

namespace Domain.Core.Models.Responses
{
    public record JDPIValidarContaClienteResponse : BaseTransactionResponse
    {
        public int resultado { get; set; }
        public string motivo { get; set; }
        public string motivoComplemento { get; set; }
        public string dtHrValidacao { get; set; }

        public JDPIValidarContaClienteResponse()
        {
                
        }
        public JDPIValidarContaClienteResponse(string result)
        {
            var _result = JsonSerializer.Deserialize<JDPIValidarContaClienteResponse>(result);

            resultado = _result.resultado;
            motivo = _result.motivo;
            motivoComplemento = _result.motivoComplemento;
            dtHrValidacao = _result.dtHrValidacao;
        }

    }
}
