using Domain.Core.Base;
using System.Text.Json;

namespace Domain.Core.Models.Responses
{
    public record JDPICreditoDevolucaoResponse : BaseTransactionResponse
    {

        public string idReqJdPi { get; set; }

        public string idCreditoSgct { get; set; }

        public string dtHrCreditoSgct { get; set; }


        public JDPICreditoDevolucaoResponse()
        {
                
        }
       

        public JDPICreditoDevolucaoResponse(string result)
        {
            var _result = JsonSerializer.Deserialize<JDPICreditoDevolucaoResponse>(result);

            idReqJdPi = _result.idReqJdPi;
            idCreditoSgct = _result.idCreditoSgct;
            dtHrCreditoSgct = _result.dtHrCreditoSgct;

        }
    }
}
