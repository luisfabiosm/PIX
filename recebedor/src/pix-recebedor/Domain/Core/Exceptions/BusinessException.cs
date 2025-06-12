using Domain.Core.Base;
using System;

namespace Domain.Core.Exceptions
{
  
    public class BusinessException : Exception
    {

        public int ErrorCode { get; } = 400;

        public List<object> erros = new List<object>();



        public BusinessException(string message)
            : base(message)
        {
            ErrorCode = 400;
            erros.Add(new BaseError(ErrorCode, message, Enums.EnumTipoErro.NEGOCIO));
        }


        public BusinessException(string message, int errorCode)
            : base(message)
        {
            ErrorCode = errorCode;
            erros.Add(new BaseError(ErrorCode, message, Enums.EnumTipoErro.NEGOCIO));
        }


        public BusinessException(string message, int errorCode, object details)
            : base(message)
        {
            ErrorCode = errorCode;
            erros.Add(new BaseError(ErrorCode, message, Enums.EnumTipoErro.NEGOCIO));

        }



    }
}