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

    public string CorrelationId { get; private init; }

    // Propriedades para compatibilidade com código existente
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool Success => Result.IsSuccess;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Message { get; private init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int ErrorCode => Result.ErrorCode;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public T Data => Result.IsSuccess ? Result.Value : default;

    // Construtores privados para forçar uso dos factory methods
    private BaseReturn(Result<T> result, string message, string correlationId)
    {
        Result = result;
        Message = message;
        CorrelationId = correlationId;
    }

    /// <summary>
    /// Cria um retorno de sucesso usando Result Pattern.
    /// </summary>
    public static BaseReturn<T> FromSuccess(T data, string message = "Success", string correlationId = null)
    {
        return new BaseReturn<T>(
            Result<T>.Success(data),
            message,
            correlationId ?? string.Empty);
    }

    /// <summary>
    /// Cria um retorno de erro usando Result Pattern.
    /// </summary>
    public static BaseReturn<T> FromFailure(string error, int errorCode = -1, string correlationId = null)
    {
        return new BaseReturn<T>(
            Result<T>.Failure(error, errorCode),
            error,
            correlationId ?? string.Empty);
    }

    /// <summary>
    /// Cria um retorno de erro a partir de uma exception (compatibilidade).
    /// </summary>
    public static BaseReturn<T> FromException(Exception exception, string correlationId = null, bool includeDetails = false)
    {
        var (message, errorCode, data) = ExtractExceptionInfo(exception, includeDetails);

        return new BaseReturn<T>(
            Result<T>.Failure(message, errorCode),
            message,
            correlationId ?? string.Empty);
    }

    /// <summary>
    /// Cria um retorno a partir de ValidationResult.
    /// </summary>
    public static BaseReturn<T> FromValidation(ValidationResult validation, string correlationId = null)
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
        return Result.IsSuccess
            ? onSuccess(Result.Value, CorrelationId)
            : onFailure(Result.Error, Result.ErrorCode, CorrelationId);
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
                400 => new BusinessException(Result.Error, Result.ErrorCode, null),
                -1 => new ValidateException(Result.Error, Result.ErrorCode, null),
                _ => new InternalException(Result.Error, Result.ErrorCode, null)
            };
        }
    }

    // Método auxiliar para extrair informações de exceptions
    private static (string message, int errorCode, object data) ExtractExceptionInfo(Exception exception, bool includeDetails)
    {
        return exception switch
        {
            BusinessException businessEx => (businessEx.Message, businessEx.ErrorCode, includeDetails ? businessEx.erros : null),
            InternalException internalEx => (internalEx.Message, internalEx.ErrorCode, includeDetails ? internalEx.erros : null),
            ValidateException validateEx => (validateEx.Message, validateEx.ErrorCode, validateEx.erros),
            _ => (exception.Message, -1, null)
        };
    }

    // Conversões implícitas para facilitar uso
    public static implicit operator bool(BaseReturn<T> result) => result.Success;
    public static implicit operator BaseReturn<T>(T data) => FromSuccess(data);
}