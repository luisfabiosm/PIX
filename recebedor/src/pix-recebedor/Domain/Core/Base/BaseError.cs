
using Domain.Core.Enums;

namespace Domain.Core.Base
{
    public record BaseError
    {
        public EnumTipoErro tipo { get; private set; }
        public int codigo { get; private set; }
        public string mensagem { get; private set; }
        public string? origem { get; private set; }


        public BaseError(int codigo, string mensagem, EnumTipoErro tipo = EnumTipoErro.SISTEMA, string origem = null)
        {
            this.codigo = codigo;
            this.mensagem = mensagem;
            this.origem = origem;
            this.tipo = tipo;
        }



    }
}
