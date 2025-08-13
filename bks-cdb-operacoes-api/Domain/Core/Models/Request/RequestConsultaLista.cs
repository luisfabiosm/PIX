using Domain.Core.Enum;
using static System.Net.Mime.MediaTypeNames;

namespace Domain.Core.Models.Request
{
    public sealed record RequestConsultaLista
    {
        private const string DefaultListaCarteira = "4";

        public RequestConsultaLista(int contaAgencia, int conta, int titularidade, string? listaCarteira = null)
        {
            Validate(contaAgencia, conta, titularidade);

            pintContaAg = contaAgencia;
            pintConta = conta;
            ptinTitularidade = titularidade;
            pvchListaCarteira = listaCarteira ?? DefaultListaCarteira;
        }


        public int pintContaAg { get; }
        public int pintConta { get; }
        public int ptinTitularidade { get; }
        
        public string pvchListaCarteira { get; }
        
        //private EnumTipoLista ptinTipoLista;

        //public void SetTipoLista(EnumTipoLista tpLista)
        //{
        //    ptinTipoLista = tpLista;
        //}

        private static void Validate(int contaAgencia, int conta, int titularidade)
        {

            if (contaAgencia <= 0)
                throw new ArgumentException("A conta de agência deve ser um número positivo.", nameof(contaAgencia));

            if (conta <= 0)
                throw new ArgumentException("A conta deve ser um número positivo.", nameof(conta));

            if (titularidade <= 0)
                throw new ArgumentException("A titularidade deve ser um número positivo.", nameof(titularidade));
        }
    }
}
