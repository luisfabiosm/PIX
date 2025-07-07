using System.Text.Json;
using System.Text.Json.Serialization;

namespace Domain.Core.Exceptions
{
    public sealed class SpsErroReturn
    {

        public byte tipoErro { get; private set; }
        public int codErro { get; private set; }
        public string msgErro { get; private set; }



        private SpsErroReturn(byte tipo, int codigo, string? mensagem)
        {
            tipoErro = tipo;
            codErro = codigo;
            msgErro = mensagem;
        }

        [JsonConstructorAttribute]

        private SpsErroReturn() { }


        public static SpsErroReturn Create(byte tipo, int codigo, string mensagem)
        {
            return new SpsErroReturn(tipo, codigo, mensagem);
        }
        public static SpsErroReturn Create(string spsReturn)
        {
            var _spsErro = ValidateReturnedErrorMessage(spsReturn);

            return new SpsErroReturn(_spsErro.tipoErro, _spsErro.codErro, _spsErro.msgErro);
        }


        private static SpsErroReturn ValidateReturnedErrorMessage(string spsReturn)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(spsReturn))
                    throw new ValidateException("Mensagem de erro em formato invalido");
                var _spsErro = JsonSerializer.Deserialize<SpsErroReturn>(spsReturn);
                return _spsErro;
            }

            catch (Exception ex)
            {
                throw new ValidateException("Mensagem de erro em formato invalido");

            }

        }



    }
}
