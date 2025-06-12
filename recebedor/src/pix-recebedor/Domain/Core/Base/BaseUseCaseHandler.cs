using Domain.Core.Exceptions;
using Domain.Core.Interfaces.Domain;
using Domain.Core.Interfaces.Outbound;
using Domain.Core.Mediator;
using System.ComponentModel.DataAnnotations;

namespace Domain.Core.Base
{
    public abstract class BaseUseCaseHandler<TTransaction, TResponse, TResult> : BaseService, IBSRequestHandler<TTransaction, TResponse>
        where TTransaction : BaseTransaction<TResponse>
        where TResponse : class
    {

        protected ValidateException _validateException;
        protected readonly IValidatorService _validateService;
        protected readonly ISPARepository _spaRepoSql;

        protected BaseUseCaseHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {

            _spaRepoSql = serviceProvider.GetRequiredService<ISPARepository>();
            _validateService = serviceProvider.GetRequiredService<IValidatorService>();
        }


        public async Task<TResponse> Handle(TTransaction transaction, CancellationToken cancellationToken)
        {
            try
            {
                string correlationId = transaction.CorrelationId;
                _loggingAdapter.LogInformation("Iniciando processamento: {RequestType}{CorrelationInfo}",
                    typeof(TTransaction).Name,
                    !string.IsNullOrEmpty(correlationId) ? $" [CorrelationId: {correlationId}]" : string.Empty);

                // Pré-processamento (validações, etc)
                await PreProcessing(transaction, cancellationToken);


                // Processamento principal
                var _result = await Processing(transaction, cancellationToken);


                // Pós-processamento (logs, cache, eventos, etc)
                await PosProcessing(transaction, _result, cancellationToken);

                // Logging de conclusão
                _loggingAdapter.LogInformation("Processamento concluído com sucesso: {RequestType}{CorrelationInfo}",
                    typeof(TTransaction).Name,
                    !string.IsNullOrEmpty(correlationId) ? $" [CorrelationId: {correlationId}]" : string.Empty);

                return _result;
            }
            catch (Exception ex)
            {
                return await HandleError("Handle", transaction, ex, cancellationToken);
            }
        }


        protected virtual async Task PreProcessing(TTransaction transaction, CancellationToken cancellationToken)
        {
            using var operationContext = _loggingAdapter.StartOperation("PreProcessing", transaction.CorrelationId);
            _loggingAdapter.LogInformation("Iniciando PreProcessing");

            await ValidateBeforeProcess(transaction, cancellationToken);
        }


        protected async Task<TResponse> Processing(TTransaction transaction, CancellationToken cancellationToken)
        {
            using var operationContext = _loggingAdapter.StartOperation($"{GetType().Name} - Processing", transaction.CorrelationId);
            _loggingAdapter.LogInformation($"{GetType().Name} Processing");

            try
            {
                //Executa o processamento especifico da Transação, é a logica de negócio
                var result = await ExecuteTransactionProcessing(transaction, cancellationToken);

                // Retornar um Response com sucesso
                return ReturnSuccessResponse(result, "Transação executada com sucesso", transaction.CorrelationId);
            }
            catch (Exception)
            {
                throw;
            }
        }


        protected abstract Task<TResult> ExecuteTransactionProcessing(TTransaction transaction, CancellationToken cancellationToken);


        protected abstract TResponse ReturnSuccessResponse(TResult result, string message, string correlationId);


        protected virtual async Task PosProcessing(TTransaction transaction, TResponse response, CancellationToken cancellationToken)
        {
            using var operationContext = _loggingAdapter.StartOperation("PosProcessing", transaction.CorrelationId);
            _loggingAdapter.LogInformation("Iniciando PosProcessing");

            if (response is BaseReturn<TResponse> baseReturn && baseReturn.IsSuccess)
            {
                await HandleSuccessfulResponse(transaction, response, cancellationToken);
            }
        }


        protected virtual Task HandleSuccessfulResponse(TTransaction transaction, TResponse response, CancellationToken cancellationToken)
        {
            //Pode ser modificada na classe que herdar para manipular se necessario no retorno
            return Task.CompletedTask;
        }


        protected virtual async Task ValidateBeforeProcess(TTransaction transaction, CancellationToken cancellationToken)
        {
            try
            {
                _validateException = new ValidateException("Ocorreu uma falha na validação da transação");

                if (transaction.Code <= 0)
                    _validateException.AddDetails(new ErrorDetails("Code", "Code é obrigatório e não pode ser 0"));

                if (string.IsNullOrEmpty(transaction.CorrelationId))
                    _validateException.AddDetails(new ErrorDetails("CorrelationId", "CorrelationId é obrigatório"));

                // Executar validações especificas
                await ValidateTransaction(transaction, cancellationToken);

            }
            catch (Exception ex) when (!(ex is ValidateException))
            {
                throw;
            }
        }


        protected virtual Task ValidateTransaction(TTransaction transaction, CancellationToken cancellationToken)
        {
            //Pode ser modificada na classe que herdar para manipular se necessario no retorno
            return Task.CompletedTask;
        }


        protected virtual async Task<TResponse> HandleError(string operation, TTransaction transaction, Exception exception, CancellationToken cancellationToken)
        {
            _loggingAdapter.LogError(operation, exception);
            return ReturnErrorResponse(exception, transaction.CorrelationId);
        }

        protected virtual async Task<string> HandleProcessingResult(string result, Exception exception = null)
        {
            if (exception is null)
                return result;

            _loggingAdapter.LogError(result, exception);
            throw exception;
        }


        protected abstract TResponse ReturnErrorResponse(Exception exception, string correlationId);


    }

}
