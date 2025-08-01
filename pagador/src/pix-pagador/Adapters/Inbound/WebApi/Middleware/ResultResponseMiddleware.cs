﻿using Domain.Core.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;

namespace Adapters.Inbound.WebApi.Middleware
{
    /// <summary>
    /// Middleware para padronização de respostas baseado no padrão Result.
    /// Centraliza a lógica de conversão do Result para IActionResult, garantindo consistência em todas as respostas da API.
    /// </summary>
    public class ResultResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ResultResponseMiddleware> _logger;

        public ResultResponseMiddleware(RequestDelegate next, ILogger<ResultResponseMiddleware> logger)
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

                // Verificar se deve interceptar o endpoint
                if (ShouldInterceptEndpoint(context))
                {
                    await HandleResultEndpointAsync(context);
                }
                else
                {
                    await _next(context);
                }
            }
            catch (BusinessException bex)
            {
                _logger.LogWarning(bex, "BusinessException capturada no middleware [CorrelationId: {CorrelationId}]",
                    GetCorrelationId(context));
                await HandleBusinessExceptionAsync(context, bex);
            }
            catch (ValidationException vex)
            {
                _logger.LogWarning(vex, "ValidationException capturada no middleware [CorrelationId: {CorrelationId}]",
                    GetCorrelationId(context));
                await HandleValidationExceptionAsync(context, vex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro não esperado capturado no middleware [CorrelationId: {CorrelationId}]",
                    GetCorrelationId(context));
                await HandleGenericExceptionAsync(context, ex);
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

        private string GetCorrelationId(HttpContext context)
        {
            return context.Items["CorrelationId"]?.ToString() ?? Guid.NewGuid().ToString();
        }

        private static bool ShouldInterceptEndpoint(HttpContext context)
        {
            // Interceptar apenas endpoints PIX que retornam BaseReturn<T>
            return context.Request.Path.StartsWithSegments("/soa/pix/api/v1");
        }

        private async Task HandleResultEndpointAsync(HttpContext context)
        {
            var originalResponseStream = context.Response.Body;

            try
            {
                using var responseStream = new MemoryStream();
                context.Response.Body = responseStream;

                await _next(context);

                // Processar a resposta capturada
                await ProcessResultResponseAsync(context, responseStream, originalResponseStream);
            }
            finally
            {
                context.Response.Body = originalResponseStream;
            }
        }

        private async Task ProcessResultResponseAsync(HttpContext context, MemoryStream responseStream, Stream originalResponseStream)
        {
            responseStream.Seek(0, SeekOrigin.Begin);
            var responseContent = await new StreamReader(responseStream).ReadToEndAsync();

            try
            {
                // Verificar se já é uma resposta de erro tratada
                if (context.Response.StatusCode >= 400)
                {
                    // Resposta de erro já processada, retornar como está
                    responseStream.Seek(0, SeekOrigin.Begin);
                    await responseStream.CopyToAsync(originalResponseStream);
                    return;
                }

                // Tentar processar como BaseReturn<T>
                await TryProcessBaseReturnAsync(context, responseContent, originalResponseStream);
            }
            catch (JsonException ex)
            {
                _logger.LogWarning(ex, "Falha ao processar resposta JSON como BaseReturn. Retornando resposta original.");

                // Se não conseguir processar, retornar resposta original
                responseStream.Seek(0, SeekOrigin.Begin);
                await responseStream.CopyToAsync(originalResponseStream);
            }
        }

        private async Task TryProcessBaseReturnAsync(HttpContext context, string responseContent, Stream originalResponseStream)
        {
            if (string.IsNullOrEmpty(responseContent))
            {
                await WriteEmptyResponseAsync(context, originalResponseStream);
                return;
            }

            // Tentar deserializar como BaseReturn<object> genérico
            using var document = JsonDocument.Parse(responseContent);
            var root = document.RootElement;

            // Verificar se é BaseReturn pelas propriedades que realmente são serializadas
            if (HasBaseReturnStructure(root))
            {
                await ProcessBaseReturnJsonAsync(context, root, originalResponseStream);
            }
            else
            {
                // Não é um BaseReturn, retornar como resposta de sucesso normal
                await WriteSuccessResponseAsync(context, responseContent, originalResponseStream);
            }
        }

        private static bool HasBaseReturnStructure(JsonElement root)
        {
            // ✅ DETECÇÃO MAIS ROBUSTA
            // BaseReturn<T> sempre tem CorrelationId, Success e ErrorCode
            bool hasCorrelationId = root.TryGetProperty("correlationId", out _) ||
                                   root.TryGetProperty("CorrelationId", out _);

            bool hasSuccess = root.TryGetProperty("success", out _) ||
                             root.TryGetProperty("Success", out _);

            bool hasErrorCode = root.TryGetProperty("errorCode", out _) ||
                               root.TryGetProperty("ErrorCode", out _);

            // ✅ É BaseReturn se tem essas 3 propriedades essenciais
            return hasCorrelationId && hasSuccess && hasErrorCode;
        }

        private async Task ProcessBaseReturnJsonAsync(HttpContext context, JsonElement root, Stream originalResponseStream)
        {
            var correlationId = GetCorrelationId(context);

            // ✅ EXTRAIR PROPRIEDADES ESSENCIAIS (case insensitive)
            if (root.TryGetProperty("correlationId", out var correlProp) ||
                root.TryGetProperty("CorrelationId", out correlProp))
            {
                correlationId = correlProp.GetString() ?? correlationId;
            }

            // ✅ VERIFICAR SUCCESS
            bool isSuccess = false;
            if (root.TryGetProperty("success", out var successProp) ||
                root.TryGetProperty("Success", out successProp))
            {
                isSuccess = successProp.GetBoolean();
            }

            // ✅ EXTRAIR ERROR CODE
            int errorCode = 0;
            if (root.TryGetProperty("errorCode", out var errorCodeProp) ||
                root.TryGetProperty("ErrorCode", out errorCodeProp))
            {
                errorCode = errorCodeProp.GetInt32();
            }

            if (isSuccess)
            {
                // ✅ SUCESSO - BUSCAR DATA
                JsonElement dataElement = default;
                if (root.TryGetProperty("data", out dataElement) ||
                    root.TryGetProperty("Data", out dataElement))
                {
                    // Data encontrada
                }

                await WriteResultSuccessAsync(context, dataElement, correlationId, originalResponseStream);
            }
            else
            {
                // ✅ ERRO - EXTRAIR DETALHES
                string error = "Erro desconhecido";
                if (root.TryGetProperty("message", out var msgProp) ||
                    root.TryGetProperty("Message", out msgProp))
                {
                    error = msgProp.GetString() ?? error;
                }

                // ✅ VERIFICAR SE TEM ERROR DETAILS (BusinessException)
                JsonElement errorDetailsElement = default;
                bool hasErrorDetails = root.TryGetProperty("errorDetails", out errorDetailsElement) ||
                                     root.TryGetProperty("ErrorDetails", out errorDetailsElement);

                await WriteResultFailureAsync(context, error, errorCode, correlationId,
                    hasErrorDetails ? errorDetailsElement : default, originalResponseStream);
            }
        }

        private async Task WriteResultSuccessAsync(HttpContext context, JsonElement data, string correlationId, Stream originalResponseStream)
        {
            context.Response.StatusCode = 200;
            context.Response.ContentType = "application/json";

            // Se data é um JsonElement válido, serializar; senão, retornar data vazia
            var responseData = data.ValueKind != JsonValueKind.Undefined ? data : (object?)null;

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };

            var responseBytes = JsonSerializer.SerializeToUtf8Bytes(responseData, jsonOptions);
            await originalResponseStream.WriteAsync(responseBytes);
        }

        private async Task WriteResultFailureAsync(HttpContext context, string error, int errorCode, string correlationId, JsonElement errorDetailsElement, Stream originalResponseStream)
        {
            var statusCode = errorCode == 400 ? 400 : 500;

            context.Response.StatusCode = statusCode;

            // Verificar se é BusinessException (tem errorDetails e errorCode == 400)
            if (errorDetailsElement.ValueKind != JsonValueKind.Undefined && errorCode == 400)
            {
                // Retornar formato específico de BusinessException
                await WriteBusinessExceptionResponseAsync(context, errorDetailsElement, originalResponseStream);
            }
            else
            {
                // Retornar formato padrão ProblemDetails
                await WriteStandardErrorResponseAsync(context, error, statusCode, correlationId, originalResponseStream);
            }
        }

        private async Task WriteBusinessExceptionResponseAsync(HttpContext context, JsonElement errorDetailsElement, Stream originalResponseStream)
        {
            context.Response.ContentType = "application/json";

            // Extrair propriedades do SpsErroReturn
            var businessErrorResponse = new Dictionary<string, object>();

            if (errorDetailsElement.TryGetProperty("tipoErro", out var tipoErroProp))
            {
                businessErrorResponse["tipo"] = tipoErroProp.GetInt32();
            }
            if (errorDetailsElement.TryGetProperty("codErro", out var codErroProp))
            {
                businessErrorResponse["codigo"] = codErroProp.GetInt32();
            }
            if (errorDetailsElement.TryGetProperty("msgErro", out var msgErroProp))
            {
                businessErrorResponse["mensagem"] = msgErroProp.GetString();
            }
            if (errorDetailsElement.TryGetProperty("origemErro", out var origemErroProp))
            {
                businessErrorResponse["origem"] = origemErroProp.GetString();
            }

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };

            var responseBytes = JsonSerializer.SerializeToUtf8Bytes(businessErrorResponse, jsonOptions);
            await originalResponseStream.WriteAsync(responseBytes);
        }

        private async Task WriteStandardErrorResponseAsync(HttpContext context, string error, int statusCode, string correlationId, Stream originalResponseStream)
        {
            context.Response.ContentType = "application/problem+json";

            var problemDetails = new
            {
                detail = error,
                status = statusCode,
                title = "Erro no processamento",
                extensions = new Dictionary<string, object?>
                {
                    ["correlationId"] = correlationId
                }
            };

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };

            var responseBytes = JsonSerializer.SerializeToUtf8Bytes(problemDetails, jsonOptions);
            await originalResponseStream.WriteAsync(responseBytes);
        }

        private async Task WriteSuccessResponseAsync(HttpContext context, string responseContent, Stream originalResponseStream)
        {
            context.Response.StatusCode = 200;
            context.Response.ContentType = "application/json";

            var responseBytes = Encoding.UTF8.GetBytes(responseContent);
            await originalResponseStream.WriteAsync(responseBytes);
        }

        private async Task WriteEmptyResponseAsync(HttpContext context, Stream originalResponseStream)
        {
            context.Response.StatusCode = 204; // No Content
            // Não escrever nada no body para 204
        }

        private async Task HandleBusinessExceptionAsync(HttpContext context, BusinessException bex)
        {
            var correlationId = GetCorrelationId(context);

            context.Response.StatusCode = 400;

            // Se tem SpsError, retornar formato específico
            if (bex.SpsError != null)
            {
                context.Response.ContentType = "application/json";

                var businessErrorResponse = new
                {
                    tipo = bex.SpsError.tipoErro,
                    codigo = bex.SpsError.codErro,
                    mensagem = bex.SpsError.msgErro,
                    origem = bex.SpsError.origemErro
                };

                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = false
                };

                var responseBytes = JsonSerializer.SerializeToUtf8Bytes(businessErrorResponse, jsonOptions);
                await context.Response.Body.WriteAsync(responseBytes);
            }
            else
            {
                // Formato padrão ProblemDetails
                context.Response.ContentType = "application/problem+json";

                var problemDetails = new
                {
                    detail = bex.Message,
                    status = 400,
                    title = "Erro no processamento",
                    extensions = new Dictionary<string, object?>
                    {
                        ["correlationId"] = correlationId
                    }
                };

                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = false
                };

                var responseBytes = JsonSerializer.SerializeToUtf8Bytes(problemDetails, jsonOptions);
                await context.Response.Body.WriteAsync(responseBytes);
            }
        }

        private async Task HandleValidationExceptionAsync(HttpContext context, ValidationException vex)
        {
            var correlationId = GetCorrelationId(context);

            context.Response.StatusCode = 400;
            context.Response.ContentType = "application/problem+json";

            var problemDetails = new
            {
                detail = vex.Message,
                status = 400,
                title = "Erro no processamento",
                extensions = new Dictionary<string, object?>
                {
                    ["correlationId"] = correlationId
                }
            };

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };

            var responseBytes = JsonSerializer.SerializeToUtf8Bytes(problemDetails, jsonOptions);
            await context.Response.Body.WriteAsync(responseBytes);
        }

        private async Task HandleGenericExceptionAsync(HttpContext context, Exception ex)
        {
            var correlationId = GetCorrelationId(context);

            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/problem+json";

            var problemDetails = new
            {
                detail = "Erro interno do servidor",
                status = 500,
                title = "Erro no processamento",
                extensions = new Dictionary<string, object?>
                {
                    ["correlationId"] = correlationId
                }
            };

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };

            var responseBytes = JsonSerializer.SerializeToUtf8Bytes(problemDetails, jsonOptions);
            await context.Response.Body.WriteAsync(responseBytes);
        }
    }

    /// <summary>
    /// Extension method para registrar o ResultResponseMiddleware
    /// </summary>
    public static class ResultResponseMiddlewareExtensions
    {
        public static IApplicationBuilder UseResultResponseMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ResultResponseMiddleware>();
        }
    }
}