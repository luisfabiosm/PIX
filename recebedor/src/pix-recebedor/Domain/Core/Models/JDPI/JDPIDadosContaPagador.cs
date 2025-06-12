using Domain.Core.Enums;

namespace Domain.Core.Models.JDPI
{
    public record JDPIDadosContaPagador
    {
        public int ispb { get; set; }
        public EnumTipoPessoa tpPessoa { get; set; }
        public long cpfCnpj { get; set; }
        public string nome { get; set; }
        public string nrAgencia { get; set; }
        public EnumTipoConta tpConta { get; set; }
        public string nrConta { get; set; }
    }
}
