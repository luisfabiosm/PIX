using Adapters.Inbound.WebApi.Pix.Mapping;
using Domain.Core.Base;
using Domain.Core.Mediator;
using Domain.Core.Models.Requests;
using Domain.Core.Models.Responses;
using Domain.UseCases.CreditoDevolucao;
using Microsoft.AspNetCore.Mvc;


namespace Adapters.Inbound.WebApi.Pix.Endpoints
{
    public static partial class DevolucaoEndpoint
    {
        public static void AddDevolucaoEndpoints(this WebApplication app)
        {

            var group = app.MapGroup("soa/pix/api/v1/devolucao")
                         .WithTags("PIX Recebedor")
                         .RequireAuthorization();


            group.MapPost("credito", async (
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
          .WithName("Efetivar Credito Devolução em Conta Cliente")
          .WithDescription("Efetivar o Credito de Devolução em Conta de Cliente")
          .Produces(StatusCodes.Status200OK)
          .Produces(StatusCodes.Status401Unauthorized)
          .Produces(StatusCodes.Status400BadRequest);
        }

    }
}
