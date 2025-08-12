using Domain.Core.Common.Serialization;
using Domain.Core.Enum;
using Domain.Core.Exceptions;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Domain.Core.Common.ResultPattern;


public record BaseReturn<T>
{
    [JsonIgnore]
    public Result<T> Result { get; private init; }

    public string CorrelationId { get; private init; } = string.Empty;
    public bool Success => Result.IsSuccess;
    public int ErrorCode => Result.ErrorCode;

    public string? Message { get; private init; }
    public T? Data => Result.IsSuccess ? Result.Value : default;
    public ErrorDetailsReturn? ErrorDetails { get; private init; }

    private BaseReturn(Result<T> result, string? message, string correlationId, ErrorDetailsReturn? errorDetails = null)
    {
        Result = result;
        Message = message;
        CorrelationId = correlationId ?? string.Empty;
        ErrorDetails = errorDetails;
    }

  
    public static BaseReturn<T> FromSuccess(T data, string? message = "Success", string? correlationId = null)
    {
        return new BaseReturn<T>(
            Result<T>.Success(data),
            message,
            correlationId ?? string.Empty);
    }

 
    public static BaseReturn<T> FromFailure(string error, int errorCode = -1, string? correlationId = null, ErrorDetailsReturn? errorDetails = null)
    {
        return new BaseReturn<T>(
            Result<T>.Failure(error, errorCode),
            error,
            correlationId ?? string.Empty,
            errorDetails);
    }


    public static BaseReturn<T> FromException(Exception exception, string? correlationId = null, bool includeDetails = false)
    {
        var (message, errorCode, errorDetails) = ExtractExceptionInfo(exception, includeDetails);

        return new BaseReturn<T>(
            Result<T>.Failure(message, errorCode),
            message,
            correlationId ?? string.Empty,
            errorDetails);
    }

    //public static BaseReturn<T> FromValidation(ValidationResult validation, string? correlationId = null)
    //{
    //    if (validation.IsValid)
    //        return FromSuccess(default, "Validation passed", correlationId);

    //    var errorMessage = string.Join("; ", validation.Errors.Select(e => e.mensagem));
    //    return FromFailure(errorMessage, 400, correlationId);

    //}

    //public TResult Match<TResult>(
    //    Func<T, string, TResult> onSuccess,
    //    Func<string, int, string, TResult> onFailure)
    //{
    //    if (Result.IsSuccess)
    //    {
    //        return onSuccess(Result.Value, CorrelationId);
    //    }
    //    else
    //    {
    //        return onFailure(Result.Error, Result.ErrorCode, CorrelationId);
    //    }
    //}


    //public Result<T> ToResult() => Result;


    private static (string message, int errorCode, ErrorDetailsReturn? errorDetails) ExtractExceptionInfo(Exception exception, bool includeDetails)
    {
        return exception switch
        {
            BusinessException businessEx => (
                businessEx.Message,
                businessEx.ErrorCode,
                businessEx.BusinessError 
            ),
            ValidateException validateEx => (
                validateEx.Message,
                validateEx.ErrorCode, 
                CreateValidateExceptionErrorDetails(validateEx) 
            ),
            _ => (exception.Message, 500, null)
        };
    }

    private static ErrorDetailsReturn? CreateValidateExceptionErrorDetails(ValidateException validateEx)
    {
        if (validateEx.RequestErrors != null && validateEx.RequestErrors.Any())
        {
            return ErrorDetailsReturn.CreateWithValidationErrors(
                tipo: (int)EnumTipoErro.SISTEMA,
                codigo: validateEx.ErrorCode,
                mensagem: validateEx.Message,
                origem: "pix-pagador -VALIDACAO REQUEST",
                validationErrors: validateEx.RequestErrors
            );
        }
        return null;
    }

    // Conversões implícitas para facilitar uso
    public static implicit operator bool(BaseReturn<T> result) => result.Success;
    public static implicit operator BaseReturn<T>(T data) => FromSuccess(data);

}