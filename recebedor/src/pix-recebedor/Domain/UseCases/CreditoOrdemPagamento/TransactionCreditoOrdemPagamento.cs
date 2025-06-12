using Domain.Core.Base;
using Domain.Core.Enums;
using Domain.Core.Models.JDPI;
using Domain.Core.Models.Responses;
using System.Text.Json;

namespace Domain.UseCases.CreditoOrdemPagamento
{
    public record TransactionCreditoOrdemPagamento : BaseTransaction<BaseReturn<JDPICreditoOrdemPagamentoResponse>>
    {
        public string idReqJdPi { get; set; }
        public string endToEndId { get; set; }
        public string idConciliacaoRecebedor { get; set; }
        public EnumTpIniciacao tpIniciacao { get; set; }
        public JDPIDadosContaPagador pagador { get; set; }
        public JDPIDadosContaRecebedor recebedor { get; set; }
        public string dtHrOp { get; set; }
        public double valor { get; set; }
        public string infEntreClientes { get; set; }
        public EnumPrioridadePagamento? prioridadePagamento { get; set; }
        public EnumTpPrioridadePagamento? tpPrioridadePagamento { get; set; }
        public EnumTipoFinalidade? finalidade { get; set; }
        public EnumModalidadeAgente? modalidadeAgente { get; set; }
        public int? ispbPss { get; set; }
        public long? cnpjIniciadorPagamento { get; set; }
        public List<JDPIValorDetalhe>? vlrDetalhe { get; set; }
        public TransactionCreditoOrdemPagamento()
        {
            
        }

        public override string getTransactionSerialization()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
