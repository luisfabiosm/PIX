using Domain.Core.Base;
using Domain.Core.Models.JDPI;
using Domain.Core.Models.Responses;
using System.Text.Json;

namespace Domain.UseCases.CreditoDevolucao
{
    public record TransactionCreditoDevolucao : BaseTransaction<BaseReturn<JDPICreditoDevolucaoResponse>>
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

        public string infEntreClientes { get; set; }

        public TransactionCreditoDevolucao()
        {
            
        }

        public override string getTransactionSerialization()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
