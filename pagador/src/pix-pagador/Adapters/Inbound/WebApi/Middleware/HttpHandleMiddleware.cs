using Domain.Core.Common.ResultPattern;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Net;

namespace Adapters.Inbound.WebApi.Middleware
{
    public class HttpHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HttpHandlingMiddleware> _logger;

        public HttpHandlingMiddleware(RequestDelegate next, ILogger<HttpHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Adicionar correlation ID ao contexto se não existir
                EnsureCorrelationId(context);

                // Interceptar apenas endpoints específicos
                if (ShouldInterceptEndpoint(context))
                {
                    await HandleInterceptedEndpointAsync(context);
                }
                else
                {
                    await _next(context);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro não tratado no HttpHandlingMiddleware");
                await HandleExceptionAsync(context, ex);
            }
        }

        private void EnsureCorrelationId(HttpContext context)
        {
            var correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault();

            if (string.IsNullOrEmpty(correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
            }

            context.Items["CorrelationId"] = correlationId;
            context.Response.Headers["X-Correlation-ID"] = correlationId;
        }

        private static bool ShouldInterceptEndpoint(HttpContext context)
        {
            // Interceptar apenas endpoints do PIX que retornam BaseReturn
            return context.Request.Path.StartsWithSegments("/soa/pix/api/v1/debito") &&
                   context.Request.Method == HttpMethods.Post;
        }

        private async Task HandleInterceptedEndpointAsync(HttpContext context)
        {
            var originalResponseStream = context.Response.Body;

            try
            {
                using var responseStream = new MemoryStream();
                context.Response.Body = responseStream;

                await _next(context);

                // Processar a resposta
                await ProcessResponseAsync(context, responseStream, originalResponseStream);
            }
            finally
            {
                context.Response.Body = originalResponseStream;
            }
        }

        private async Task ProcessResponseAsync(HttpContext context, MemoryStream responseStream, Stream originalResponseStream)
        {
            responseStream.Seek(0, SeekOrigin.Begin);
            var responseContent = await new StreamReader(responseStream).ReadToEndAsync();

            try
            {
                // Verificar se é uma resposta de erro (Problem Details)
                if (context.Response.StatusCode >= 400)
                {
                    await HandleErrorResponseAsync(context, responseContent, originalResponseStream);
                    return;
                }

                // Tentar processar como resposta de sucesso
                await HandleSuccessResponseAsync(context, responseContent, originalResponseStream);
            }
            catch (JsonException ex)
            {
                _logger.LogWarning(ex, "Falha ao processar resposta JSON. Retornando resposta original.");

                // Se não conseguir processar, retornar resposta original
                responseStream.Seek(0, SeekOrigin.Begin);
                await responseStream.CopyToAsync(originalResponseStream);
            }
        }

        private async Task HandleSuccessResponseAsync(HttpContext context, string responseContent, Stream originalResponseStream)
        {
            var correlationId = context.Items["CorrelationId"]?.ToString() ?? Guid.NewGuid().ToString();

            object? data = null;

            if (!string.IsNullOrEmpty(responseContent))
            {
                try
                {
                    // Tentar deserializar como objeto genérico
                    data = JsonSerializer.Deserialize<object>(responseContent);
                }
                catch
                {
                    // Se falhar, usar o conteúdo como string
                    data = responseContent;
                }
            }

            var standardResponse = new StandardApiResponse
            {
                Success = true,
                Data = data,
                CorrelationId = correlationId,
                Timestamp = DateTime.UtcNow,
                Path = context.Request.Path
            };

            await WriteStandardResponseAsync(context, standardResponse, originalResponseStream);
        }

        private async Task HandleErrorResponseAsync(HttpContext context, string responseContent, Stream originalResponseStream)
        {
            var correlationId = context.Items["CorrelationId"]?.ToString() ?? Guid.NewGuid().ToString();

            ApiError error;

            try
            {
                // Tentar extrair detalhes do ProblemDetails
                var problemDetails = JsonSerializer.Deserialize<ProblemDetailsResponse>(responseContent);

                error = new ApiError
                {
                    Code = DetermineErrorCode(context.Response.StatusCode),
                    Message = problemDetails?.Title ?? "Erro no processamento",
                    Details = problemDetails?.Detail
                };

                // Tentar extrair correlation ID do extensions se disponível
                if (problemDetails?.Extensions?.TryGetValue("correlationId", out var correlIdFromProblem) == true)
                {
                    correlationId = correlIdFromProblem?.ToString() ?? correlationId;
                }
            }
            catch
            {
                // Se falhar ao processar ProblemDetails, criar erro genérico
                error = new ApiError
                {
                    Code = DetermineErrorCode(context.Response.StatusCode),
                    Message = "Erro no processamento",
                    Details = responseContent
                };
            }

            var standardResponse = new StandardApiResponse
            {
                Success = false,
                Error = error,
                CorrelationId = correlationId,
                Timestamp = DateTime.UtcNow,
                Path = context.Request.Path
            };

            await WriteStandardResponseAsync(context, standardResponse, originalResponseStream);
        }

        private static string DetermineErrorCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "BAD_REQUEST",
                401 => "UNAUTHORIZED",
                403 => "FORBIDDEN",
                404 => "NOT_FOUND",
                409 => "CONFLICT",
                422 => "UNPROCESSABLE_ENTITY",
                500 => "INTERNAL_SERVER_ERROR",
                502 => "BAD_GATEWAY",
                503 => "SERVICE_UNAVAILABLE",
                _ => "UNKNOWN_ERROR"
            };
        }

        private async Task WriteStandardResponseAsync(HttpContext context, StandardApiResponse response, Stream originalResponseStream)
        {
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            var jsonResponse = JsonSerializer.Serialize(response, jsonOptions);
            var responseBytes = System.Text.Encoding.UTF8.GetBytes(jsonResponse);

            context.Response.ContentType = "application/json";
            context.Response.ContentLength = responseBytes.Length;

            await originalResponseStream.WriteAsync(responseBytes);
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "Erro não tratado durante o processamento da requisição");

            var correlationId = context.Items["CorrelationId"]?.ToString() ?? Guid.NewGuid().ToString();

            var response = new StandardApiResponse
            {
                Success = false,
                Error = new ApiError
                {
                    Code = "INTERNAL_ERROR",
                    Message = "Erro interno do servidor",
                    Details = exception.Message
                },
                CorrelationId = correlationId,
                Timestamp = DateTime.UtcNow,
                Path = context.Request.Path
            };

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };

            var jsonResponse = JsonSerializer.Serialize(response, jsonOptions);
            var responseBytes = System.Text.Encoding.UTF8.GetBytes(jsonResponse);

            await context.Response.Body.WriteAsync(responseBytes);
        }
    }

    // Modelos auxiliares
    public class ProblemDetailsResponse
    {
        public string? Title { get; set; }
        public string? Detail { get; set; }
        public int Status { get; set; }
        public Dictionary<string, object>? Extensions { get; set; }
    }

    public class StandardApiResponse
    {
        public bool Success { get; set; }
        public object? Data { get; set; }
        public ApiError? Error { get; set; }
        public string CorrelationId { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string Path { get; set; } = string.Empty;
    }

    public class ApiError
    {
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? Details { get; set; }
    }

    // Extension method para registrar o middleware
    public static class HttpHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseHttpHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HttpHandlingMiddleware>();
        }
    }
}