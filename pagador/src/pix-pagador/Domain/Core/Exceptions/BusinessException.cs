

using Domain.Core.Enum;

namespace Domain.Core.Exceptions
{

    public class BusinessException : Exception
    {

        public int ErrorCode { get; } = 400;

        public SpsErroReturn SpsError = null;


        public BusinessException(string mensagem) : base(mensagem)
        {

        }

        public static BusinessException Create(string mensagem, int codigo, string origem = "API")
        {
            var _bexception = new BusinessException(mensagem);
            _bexception.SpsError = new SpsErroReturn
            {
                codErro = codigo,
                msgErro = mensagem,
                origemErro = origem,
                tipoErro = (int)EnumTipoErro.NEGOCIO
            };
            return _bexception;
        }

        public static BusinessException Create(SpsErroReturn spsReturn)
        {
            var _bexception = new BusinessException(spsReturn.msgErro);
            _bexception.SpsError = spsReturn;
            return _bexception;
        }



        //public BusinessException(SpsErroReturn spsReturn, string? mensagem = null) : base(mensagem)
        //{
        //    ErrorCode = spsReturn.tipoErro == 1 ? ErrorCode : 500;
        //    mensagem = spsReturn.msgErro;
        //    SpsError = spsReturn;
        //}


        //public BusinessException(string mensagem, int codigo, byte tipo) : base(mensagem)
        //{
        //    ErrorCode = tipo == 1 ? ErrorCode : 500;
        //    SpsError = SpsErroReturn.Create(tipo, codigo, mensagem);
        //}

        //public BusinessException(string mensagem, int codigo) : base(mensagem)
        //{
        //    SpsError = SpsErroReturn.Create((int)EnumTipoErro.NEGOCIO, codigo, mensagem);
        //}



    }
}