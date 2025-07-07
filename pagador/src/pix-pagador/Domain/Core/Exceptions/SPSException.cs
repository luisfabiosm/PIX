namespace Domain.Core.Exceptions
{

    public class SPSException : Exception
    {

        public int ErrorCode { get; internal set; } = 400;

        public SpsErroReturn Error = null;

        public SPSException(SpsErroReturn spsReturn, string? mensagem = null) : base(mensagem)
        {
            ErrorCode = spsReturn.tipoErro == 1 ? ErrorCode : 500;
            Error = spsReturn;
        }


        public SPSException(string mensagem, int codigo, byte tipo)
            : base(mensagem)
        {
            ErrorCode = tipo == 1 ? ErrorCode : 500;
            Error = SpsErroReturn.Create(tipo, codigo, mensagem);
        }




    }
}