using Adapters.Inbound.WebApi.Pix.Mapping;
using Domain.Core.Base;
using Domain.Core.Mediator;
using Domain.Core.Models.Request;
using Microsoft.AspNetCore.Mvc;


namespace Adapters.Inbound.WebApi.Pix.Endpoints
{
    public static partial class DevolucaoEndpoint
    {
        public static void AddDevolucaoEndpoints(this WebApplication app)
        {

            var group = app.MapGroup("soa/pix/api/v1/devolucao")
                         .WithTags("PIX Pagador")
                         .RequireAuthorization();


            group.MapPost("requisitar", async (
                  [FromBody] JDPICreditoDevolucaoRequest request,
                  [FromServices] BSMediator bSMediator,
                  [FromServices] MappingHttpRequestToTransaction mapping
                  ) =>
            {
                      string correlationId = Guid.NewGuid().ToString();
                      var transaction = mapping.ToTransactionCreditoDevolucao(request, correlationId, 1);
                      var _result = await bSMediator.Send<TransactionCreditoDevolucao, BaseReturn<JDPICreditoDevolucaoResponse>>(transaction);

                      if (!_result.Success)
                          _result.ThrowIfError();

                      return Results.Ok(_result.Data);
            })
                  .WithName("Requisitar Ordem Devolução")
                  .WithDescription("Requisitar inicio de Ordem de Devolução")
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
                  .WithName("Cancelar Ordem Devolução")
                  .WithDescription("Cancelar Ordem de Devolução iniciada")
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
               .WithName("Efetivar Ordem Devolução")
               .WithDescription("Efetivar Ordem de Devolução registrada")
               .Produces(StatusCodes.Status200OK)
               .Produces(StatusCodes.Status401Unauthorized)
               .Produces(StatusCodes.Status400BadRequest);
        }

    }
}
