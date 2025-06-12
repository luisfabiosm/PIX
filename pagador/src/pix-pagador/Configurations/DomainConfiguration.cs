

using Domain.Core.Base;
using Domain.Core.Interfaces.Domain;
using Domain.Core.Mediator;
using Domain.Core.Models.Responses;
using Domain.Services;
using Domain.UseCases.Pagamento.EfetivarOrdemPagamento;
using Domain.UseCases.Pagamento.RegistrarOrdemPagamento;
using System.Reflection;

namespace Configurations 
{
    public static class DomainConfiguration
    {

        public static IServiceCollection ConfigureDomainAdapters(this IServiceCollection services, IConfiguration configuration)
        {


            #region Domain MediatoR

            services.AddTransient<BSMediator>();
            services.AddTransient<IBSRequestHandler<TransactionRegistrarOrdemPagamento, BaseReturn<JDPIRegistrarOrdemPagamentoResponse>>, RegistrarOrdemPagamentoHandler>(); //PARA CADA USECASE HANDLER
            services.AddTransient<IBSRequestHandler<TransactionEfetivarOrdemPagamento, BaseReturn<JDPIEfetivarOrdemPagamentoResponse>>, EfetivarOrdemPagamentoHandler>(); //PARA CADA USECASE HANDLER


            #endregion


            #region Domain Services

            services.AddScoped<IValidatorService, ValidatorService>();

            #endregion



            return services;
        }
    }
}
