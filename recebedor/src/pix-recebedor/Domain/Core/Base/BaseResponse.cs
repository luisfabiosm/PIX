namespace Domain.Core.Base
{
    public class BaseResponse<T>
    {
        public string mensagem { get; set; }
        public int codigo { get; set; }
        public T erros { get; set; }

        public static BaseResponse<T> CreateSuccess(T data, string message = "Sucesso")
        {
            return new BaseResponse<T>
            {
                erros = data,
                mensagem = message,
              
            };
        }

        public static BaseResponse<T> CreateError(string message, int? errorCode = null)
        {
            return new BaseResponse<T>
            {
                mensagem = message,
                codigo = errorCode??-1
            };
        }
    }
}
