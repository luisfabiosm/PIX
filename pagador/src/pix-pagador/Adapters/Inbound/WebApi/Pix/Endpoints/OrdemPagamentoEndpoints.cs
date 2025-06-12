using Adapters.Inbound.WebApi.Pix.Mapping;
using Domain.Core.Base;
using Domain.Core.Mediator;
using Domain.Core.Models.Request;
using Domain.Core.Models.Responses;
using Domain.UseCases.Pagamento.CancelarOrdemPagamento;
using Domain.UseCases.Pagamento.EfetivarOrdemPagamento;
using Domain.UseCases.Pagamento.RegistrarOrdemPagamento;
using Microsoft.AspNetCore.Mvc;



namespace Adapters.Inbound.WebApi.Pix.Endpoints
{
    public static partial class OrdemPagamentoEndpoints
    {

        public static void AddOrdemPagamentoEndpoints(this WebApplication app)
        {

            var group = app.MapGroup("soa/pix/api/v1/debito")
                         .WithTags("PIX Pagador")
                         .RequireAuthorization();


            group.MapPost("registrar", async (
                    [FromBody] JDPIRegistrarOrdemPagtoRequest request,
                    [FromServices] BSMediator bSMediator,
                    [FromServices] MappingHttpRequestToTransaction mapping
                    ) =>
                {
                    string correlationId = Guid.NewGuid().ToString();
                    var transaction = mapping.ToTransactionRegistrarOrdemPagamento(request, correlationId, 1);
                    var _result = await bSMediator.Send<TransactionRegistrarOrdemPagamento, BaseReturn<JDPIRegistrarOrdemPagamentoResponse>>(transaction);

                    if (!_result.Success)
                        _result.ThrowIfError();

                    return Results.Ok(_result.Data);
                })
                .WithName("Registrar Ordem Pagamento")
                .WithDescription("Iniciar registrar de Ordem de Pagamento")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status400BadRequest);



            group.MapPost("cancelar", async (
                    [FromBody] JDPICancelarRegistroOrdemPagtoRequest request,
                    [FromServices] BSMediator bSMediator,
                    [FromServices] MappingHttpRequestToTransaction mapping
                    ) =>
                {
                    string correlationId = Guid.NewGuid().ToString();
                    var transaction = mapping.ToTransactionCancelarOrdemPagamento(request, correlationId, 1);
                    var _result = await bSMediator.Send<TransactionCancelarOrdemPagamento, BaseReturn<JDPICancelarOrdemPagamentoResponse>>(transaction);

                    if (!_result.Success)
                        _result.ThrowIfError();

                    return Results.Ok(_result.Data);
                })
                .WithName("Cancelar Ordem Pagamento")
                .WithDescription("Cancelar Ordem de Pagamento registrada")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status400BadRequest);



            group.MapPost("efetivar", async (
                   [FromBody] JDPIEfetivarOrdemPagtoRequest request,
                   [FromServices] BSMediator bSMediator,
                   [FromServices] MappingHttpRequestToTransaction mapping
                   ) =>
                        {
                            string correlationId = Guid.NewGuid().ToString();
                            var transaction = mapping.ToTransactionEfetivarOrdemPagamento(request, correlationId, 1);
                            var _result = await bSMediator.Send<TransactionEfetivarOrdemPagamento, BaseReturn<JDPIEfetivarOrdemPagamentoResponse>>(transaction);

                            if (!_result.Success)
                                _result.ThrowIfError();

                            return Results.Ok(_result.Data);
                        })
               .WithName("Efetivar Ordem Pagamento")
               .WithDescription("Efetivar Ordem de Pagamento registrada")
               .Produces(StatusCodes.Status200OK)
               .Produces(StatusCodes.Status401Unauthorized)
               .Produces(StatusCodes.Status400BadRequest);
        }
    }
}

