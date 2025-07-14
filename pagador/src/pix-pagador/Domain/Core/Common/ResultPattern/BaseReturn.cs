using Domain.Core.Exceptions;
using System.Text.Json.Serialization;

namespace Domain.Core.Common.ResultPattern;

/// <summary>
/// Classe base para retornos da aplicação usando Result Pattern.
/// Elimina o uso de exceptions para controle de fluxo.
/// </summary>
/// <typeparam name="T">Tipo dos dados de retorno</typeparam>
public record BaseReturn<T>
{
    [JsonIgnore]
    public Result<T> Result { get; private init; }

    // ✅ PROPRIEDADES SEMPRE SERIALIZADAS PARA O MIDDLEWARE DETECTAR
    public string CorrelationId { get; private init; } = string.Empty;
    public bool Success => Result.IsSuccess;
    public int ErrorCode => Result.ErrorCode;

    // ✅ PROPRIEDADES CONDICIONAIS
    public string? Message { get; private init; }
    public T? Data => Result.IsSuccess ? Result.Value : default;
    public SpsErroReturn? ErrorDetails { get; private init; }

    // Construtores privados para forçar uso dos factory methods
    private BaseReturn(Result<T> result, string? message, string correlationId, SpsErroReturn? errorDetails = null)
    {
        Result = result;
        Message = message;
        CorrelationId = correlationId ?? string.Empty;
        ErrorDetails = errorDetails;
    }

    /// <summary>
    /// Cria um retorno de sucesso usando Result Pattern.
    /// </summary>
    public static BaseReturn<T> FromSuccess(T data, string? message = "Success", string? correlationId = null)
    {
        return new BaseReturn<T>(
            Result<T>.Success(data),
            message,
            correlationId ?? string.Empty);
    }

    /// <summary>
    /// Cria um retorno de erro usando Result Pattern.
    /// </summary>
    public static BaseReturn<T> FromFailure(string error, int errorCode = -1, string? correlationId = null, SpsErroReturn? errorDetails = null)
    {
        return new BaseReturn<T>(
            Result<T>.Failure(error, errorCode),
            error,
            correlationId ?? string.Empty,
            errorDetails);
    }

    /// <summary>
    /// Cria um retorno de erro a partir de uma exception.
    /// ✅ AUTOMATICAMENTE EXTRAI SpsError DE BusinessException
    /// </summary>
    public static BaseReturn<T> FromException(Exception exception, string? correlationId = null, bool includeDetails = false)
    {
        var (message, errorCode, errorDetails) = ExtractExceptionInfo(exception, includeDetails);

        return new BaseReturn<T>(
            Result<T>.Failure(message, errorCode),
            message,
            correlationId ?? string.Empty,
            errorDetails);
    }

    /// <summary>
    /// Cria um retorno a partir de ValidationResult.
    /// </summary>
    public static BaseReturn<T> FromValidation(ValidationResult validation, string? correlationId = null)
    {
        if (validation.IsValid)
            return FromSuccess(default, "Validation passed", correlationId);

        var errorMessage = string.Join("; ", validation.Errors.Select(e => e.mensagens));
        return FromFailure(errorMessage, 400, correlationId);
    }

    /// <summary>
    /// Pattern matching para Result.
    /// Elimina necessidade de if/else e melhora performance.
    /// </summary>
    public TResult Match<TResult>(
        Func<T, string, TResult> onSuccess,
        Func<string, int, string, TResult> onFailure)
    {
        if (Result.IsSuccess)
        {
            return onSuccess(Result.Value, CorrelationId);
        }
        else
        {
            return onFailure(Result.Error, Result.ErrorCode, CorrelationId);
        }
    }

    /// <summary>
    /// Converte para Result<T> puro.
    /// </summary>
    public Result<T> ToResult() => Result;

    /// <summary>
    /// DEPRECATED: Mantido apenas para compatibilidade.
    /// Use pattern matching ao invés de exceptions.
    /// </summary>
    [Obsolete("Use pattern matching com Match() ao invés de exceptions", false)]
    public void ThrowIfError()
    {
        if (!Result.IsSuccess)
        {
            throw Result.ErrorCode switch
            {
                400 => new BusinessException(Result.Error),
                -1 => new ValidateException(Result.Error, Result.ErrorCode, null),
                _ => new InternalException(Result.Error, Result.ErrorCode, null)
            };
        }
    }

    /// <summary>
    /// ✅ MÉTODO APRIMORADO PARA EXTRAIR INFORMAÇÕES DE EXCEPTIONS
    /// </summary>
    private static (string message, int errorCode, SpsErroReturn? errorDetails) ExtractExceptionInfo(Exception exception, bool includeDetails)
    {
        return exception switch
        {
            BusinessException businessEx => (
                businessEx.Message,
                businessEx.ErrorCode,
                businessEx.SpsError // ✅ SEMPRE INCLUI SpsError SE EXISTIR
            ),
            InternalException internalEx => (
                internalEx.Message,
                internalEx.ErrorCode,
                null // InternalException não tem SpsError
            ),
            ValidateException validateEx => (
                validateEx.Message,
                validateEx.ErrorCode,
                null // ValidateException não tem SpsError
            ),
            _ => (exception.Message, -1, null)
        };
    }

    // Conversões implícitas para facilitar uso
    public static implicit operator bool(BaseReturn<T> result) => result.Success;
    public static implicit operator BaseReturn<T>(T data) => FromSuccess(data);

    /// <summary>
    /// ✅ MÉTODO PARA VERIFICAR SE É BusinessException (usado pelo middleware)
    /// </summary>
    [JsonIgnore]
    public bool IsBusinessException => ErrorDetails != null && ErrorCode == 400;
}