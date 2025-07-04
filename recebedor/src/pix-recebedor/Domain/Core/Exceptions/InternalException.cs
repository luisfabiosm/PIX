﻿using Domain.Core.Base;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Domain.Core.Exceptions
{
  
    public class InternalException : Exception
    {

        public int ErrorCode { get; } = 1;

        public List<object> erros = new List<object>(); 
        public InternalException(string message)
            : base(message)
        {
            erros.Add(new BaseError(ErrorCode,message,Enums.EnumTipoErro.SISTEMA));
            ErrorCode = -1;
        }


        public InternalException(string message, int errorCode, object details)
            : base(message)
        {
            erros.Add(new BaseError(errorCode, message, Enums.EnumTipoErro.SISTEMA));
            ErrorCode = errorCode;
        }

        public InternalException(string message, int errorCode, Exception innerException)
            : base(message, innerException)
        {
            erros.Add(new BaseError(ErrorCode, message, Enums.EnumTipoErro.SISTEMA));
            ErrorCode = errorCode;
        }

    }
}