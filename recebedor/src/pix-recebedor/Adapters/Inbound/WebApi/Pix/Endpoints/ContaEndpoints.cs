using Adapters.Inbound.WebApi.Pix.Mapping;
using Domain.Core.Base;
using Domain.Core.Mediator;
using Domain.Core.Models.Requests;
using Domain.Core.Models.Responses;
using Domain.UseCases.ValidarContaCliente;
using Microsoft.AspNetCore.Mvc;



namespace Adapters.Inbound.WebApi.Pix.Endpoints
{
    public static partial class ContaEndpoints
    {

        public static void AddContaEndpoints(this WebApplication app)
        {

            var group = app.MapGroup("soa/pix/api/v1/credito")
                         .WithTags("PIX Recebedor")
                         .RequireAuthorization();


            group.MapPost("validar", async (
                [FromBody] JDPIValidarContaRequest request,
                [FromServices] BSMediator bSMediator,
                [FromServices] MappingHttpRequestToTransaction mapping
                ) =>
            {
                string correlationId = Guid.NewGuid().ToString();
                var transaction = mapping.ToTransactionValidarContaCliente(request, correlationId, 1);
                var _result = await bSMediator.Send<TransactionValidarContaCliente, BaseReturn<JDPIValidarContaClienteResponse>>(transaction);

                if (!_result.Success)
                    _result.ThrowIfError();

                return Results.Ok(_result.Data);
            })
            .WithName("Validar Conta Cliente")
            .WithDescription("Validar Conta de Cliente Recebedor de Crédito Pagamento ou Devolução")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status400BadRequest);



        }
    }
}

