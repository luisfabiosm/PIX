using Adapters.Inbound.WebApi.Pix.Mapping;
using Domain.Core.Base;
using Domain.Core.Mediator;
using Domain.Core.Models.Requests;
using Domain.Core.Models.Responses;
using Domain.UseCases.CreditoOrdemPagamento;
using Microsoft.AspNetCore.Mvc;

namespace Adapters.Inbound.WebApi.Pix.Endpoints
{

    public static partial class OrdemPagamentoEndpoint
    {
        public static void AddOrdemPagamentoEndpoints(this WebApplication app)
        {

            var group = app.MapGroup("soa/pix/api/v1/credito")
                         .WithTags("PIX Recebedor")
                         .RequireAuthorization();


            group.MapPost("", async (
              [FromBody] JDPICreditoOrdemPagamentoRequest request,
              [FromServices] BSMediator bSMediator,
              [FromServices] MappingHttpRequestToTransaction mapping
              ) =>
            {
                string correlationId = Guid.NewGuid().ToString();
                var transaction = mapping.ToTransactionCreditoOrdemPagamento(request, correlationId, 1);
                var _result = await bSMediator.Send<TransactionCreditoOrdemPagamento, BaseReturn<JDPICreditoOrdemPagamentoResponse>>(transaction);

                if (!_result.Success)
                    _result.ThrowIfError();

                return Results.Ok(_result.Data);
            })
          .WithName("Efetivar Credito Ordem Pagamento em Conta Cliente")
          .WithDescription("Efetivar o Credito de Ordem Pagamento em  Conta de Cliente")
          .Produces(StatusCodes.Status200OK)
          .Produces(StatusCodes.Status401Unauthorized)
          .Produces(StatusCodes.Status400BadRequest);
        }

    }
}