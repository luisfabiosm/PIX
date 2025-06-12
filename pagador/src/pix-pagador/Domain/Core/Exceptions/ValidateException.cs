using MongoDB.Bson.Serialization.Conventions;
using System;

namespace Domain.Core.Exceptions
{

    public class ValidateException : Exception
    {
        public int ErrorCode { get; internal set; } = -1;

        public List<ErrorDetails> erros { get; private set; }

        public ValidateException()
        {
            
        }
        public void AddDetails(ErrorDetails details)
        {
            this.erros.Add(details);
        }

        public ValidateException(string message)
            : base(message)
        {
            this.erros = new List<ErrorDetails>();
        }

        public ValidateException(string message, int errorCode, object details)
           : base(message)
        {
            this.ErrorCode = errorCode == -1 ? 400 : errorCode;
            erros = (List<ErrorDetails>)details;
        }

    }
}