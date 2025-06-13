using Adapters.Inbound.WebApi.Pix.Mapping;
using Domain.Core.Base;
using Domain.Core.Mediator;
using Domain.Core.Models.Request;
using Domain.Core.Models.Response;
using Domain.UseCases.Devolucao.CancelarOrdemDevolucao;
using Domain.UseCases.Devolucao.EfetivarOrdemDevolucao;
using Domain.UseCases.Devolucao.RegistrarOrdemDevolucao;
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
                  [FromBody] JDPIRequisitarDevolucaoOrdemPagtoRequest request,
                  [FromServices] BSMediator bSMediator,
                  [FromServices] MappingHttpRequestToTransaction mapping
                  ) =>
            {
                      string correlationId = Guid.NewGuid().ToString();
                      var transaction = mapping.ToTransactionRegistrarOrdemDevolucao(request, correlationId, 1);
                      var _result = await bSMediator.Send<TransactionRegistrarOrdemDevolucao, BaseReturn<JDPIRegistrarOrdemDevolucaoResponse>>(transaction);

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
                  [FromBody] JDPICancelarRegistroOrdemdDevolucaoRequest request,
                  [FromServices] BSMediator bSMediator,
                  [FromServices] MappingHttpRequestToTransaction mapping
                  ) =>
            {
                      string correlationId = Guid.NewGuid().ToString();
                      var transaction = mapping.ToTransactionCancelarOrdemDevolucao(request, correlationId, 1);
                      var _result = await bSMediator.Send<TransactionCancelarOrdemDevolucao, BaseReturn<JDPICancelarOrdemDevolucaoResponse>>(transaction);

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
                  [FromBody] JDPIEfetivarOrdemDevolucaoRequest request,
                  [FromServices] BSMediator bSMediator,
                  [FromServices] MappingHttpRequestToTransaction mapping
                 ) =>
            {
                string correlationId = Guid.NewGuid().ToString();
                var transaction = mapping.ToTransactionEfetivarOrdemDevolucao(request, correlationId, 1);
                var _result = await bSMediator.Send<TransactionEfetivarOrdemDevolucao, BaseReturn<JDPIEfetivarOrdemDevolucaoResponse>>(transaction);

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
