using Domain.Core.Models.JDPI;

namespace Domain.Core.Models.Requests
{
    public record JDPICreditoDevolucaoRequest
    {
        public string idReqJdPi { get; set; }

        public string endToEndIdOriginal { get; set; }

        public string endToEndIdDevolucao { get; set; }

        public string codigoDevolucao { get; set; }

        public string motivoDevolucao { get; set; }

        public JDPIDadosContaPagador pagador { get; set; }

        public JDPIDadosContaRecebedor recebedor { get; set; }

        public string dtHrOp { get; set; }

        public double valor { get; set; }

        public string infEntreClientes
        {
            get; set;
        }
    }

}