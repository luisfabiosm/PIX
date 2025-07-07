using Domain.Core.Common.Mediator;
using Domain.Core.Common.ResultPattern;
using Domain.Core.Common.Serialization;
using Domain.Core.Models.Response;
using Domain.Core.Ports.Domain;
using Domain.Services;
using Domain.UseCases.Devolucao.CancelarOrdemDevolucao;
using Domain.UseCases.Devolucao.EfetivarOrdemDevolucao;
using Domain.UseCases.Devolucao.RegistrarOrdemDevolucao;
using Domain.UseCases.Pagamento.CancelarOrdemPagamento;
using Domain.UseCases.Pagamento.EfetivarOrdemPagamento;
using Domain.UseCases.Pagamento.RegistrarOrdemPagamento;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Configurations
{
    public static class DomainConfiguration
    {

        public static IServiceCollection ConfigureDomainAdapters(this IServiceCollection services, IConfiguration configuration)
        {

            #region Performance Services 

            // CorrelationId Generator otimizado
            services.AddSingleton<CorrelationIdGenerator>();

            // Configuração de logging otimizado
            services.Configure<LoggerFilterOptions>(options =>
            {
                // Reduz overhead de logging em produção
                options.MinLevel = LogLevel.Information;
            });

            #endregion


            #region Domain MediatoR

            services.AddTransient<BSMediator>();

            //PAGAMENTO
            services.AddTransient<IBSRequestHandler<TransactionRegistrarOrdemPagamento, BaseReturn<JDPIRegistrarOrdemPagamentoResponse>>, RegistrarOrdemPagamentoHandler>(); //PARA CADA USECASE HANDLER
            services.AddTransient<IBSRequestHandler<TransactionEfetivarOrdemPagamento, BaseReturn<JDPIEfetivarOrdemPagamentoResponse>>, EfetivarOrdemPagamentoHandler>(); //PARA CADA USECASE HANDLER
            services.AddTransient<IBSRequestHandler<TransactionCancelarOrdemPagamento, BaseReturn<JDPICancelarOrdemPagamentoResponse>>, CancelarOrdemPagamentoHandler>(); //PARA CADA USECASE HANDLER

            //DEVOLUCAO
            services.AddTransient<IBSRequestHandler<TransactionRegistrarOrdemDevolucao, BaseReturn<JDPIRegistrarOrdemDevolucaoResponse>>, RegistrarOrdemDevolucaoHandler>(); //PARA CADA USECASE HANDLER
            services.AddTransient<IBSRequestHandler<TransactionEfetivarOrdemDevolucao, BaseReturn<JDPIEfetivarOrdemDevolucaoResponse>>, EfetivarOrdemDevolucaoHandler>(); //PARA CADA USECASE HANDLER
            services.AddTransient<IBSRequestHandler<TransactionCancelarOrdemDevolucao, BaseReturn<JDPICancelarOrdemDevolucaoResponse>>, CancelarOrdemDevolucaoHandler>(); //PARA CADA USECASE HANDLER


            #endregion


            #region Domain Services

            services.AddScoped<IValidatorService, ValidatorService>();
            services.AddScoped<ContextAccessorService>();


            #endregion

            #region JSON Serialization Optimization

            // Configurar Source Generators para System.Text.Json
            services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.TypeInfoResolver = ApiJsonContext.Default;
                options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.SerializerOptions.WriteIndented = false;
                options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });


            #endregion

            return services;
        }
    }
}
