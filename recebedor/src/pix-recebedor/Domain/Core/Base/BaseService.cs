﻿using Domain.Core.Exceptions;
using Domain.Core.Interfaces.Outbound;
using System;

namespace Domain.Core.Base
{
    public class BaseService
    {

        protected readonly ILoggingAdapter _loggingAdapter;

        public BaseService(IServiceProvider serviceProvider)
        {
            _loggingAdapter = serviceProvider.GetRequiredService<ILoggingAdapter>();
        }

        protected Exception HandleError(Exception exception, string methodName)
        {
            LogError(methodName, exception);
            return IsKnownException(exception) ? exception : WrapUnknownException(exception);
        }

        protected (string operation, Exception ex) HandleException(string methodName, Exception exception )
        {
            LogError(methodName, exception);
            return (methodName, IsKnownException(exception) ? exception : WrapUnknownException(exception));
        }


        private void LogError(string methodName, Exception exception)
        {
            _loggingAdapter.LogError($"Erro em: {methodName} - {exception.Message}", exception);
        }

        private static bool IsKnownException(Exception exception)
        {
            return exception is BusinessException or InternalException or ValidateException;
        }

        private static Exception WrapUnknownException(Exception exception)
        {
            return new InternalException(
                exception.Message ?? "Erro interno não esperado",
                1,
                exception);
        }


    }
}
