

using Domain.Core.Base;
using Domain.Core.Interfaces.Domain;
using Domain.Core.Mediator;
using Domain.Core.Models.Responses;
using Domain.Services;
using Domain.UseCases.CreditoDevolucao;
using Domain.UseCases.CreditoOrdemPagamento;
using Domain.UseCases.ValidarContaCliente;


namespace Configurations 
{
    public static class DomainConfiguration
    {

        public static IServiceCollection ConfigureDomainAdapters(this IServiceCollection services, IConfiguration configuration)
        {


            #region Domain MediatoR

            services.AddTransient<BSMediator>();
            services.AddTransient<IBSRequestHandler<TransactionValidarContaCliente, BaseReturn<JDPIValidarContaClienteResponse>>, ValidarContaClienteHandler>();
            services.AddTransient<IBSRequestHandler<TransactionCreditoOrdemPagamento, BaseReturn<JDPICreditoOrdemPagamentoResponse>>, CreditoOrdemPagamentoHandler>();
            services.AddTransient<IBSRequestHandler<TransactionCreditoDevolucao, BaseReturn<JDPICreditoDevolucaoResponse>>, CreditoDevolucaoHandler>();

            #endregion


            #region Domain Services

            services.AddScoped<IValidatorService, ValidatorService>();

            #endregion



            return services;
        }
    }
}
