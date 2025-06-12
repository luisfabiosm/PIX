using Domain.Core.Base;
using Domain.Core.Models.Responses;
using Domain.UseCases.CreditoOrdemPagamento;

namespace Domain.UseCases.CreditoDevolucao
{
    public class CreditoDevolucaoHandler : BaseUseCaseHandler<TransactionCreditoDevolucao, BaseReturn<JDPICreditoDevolucaoResponse>, JDPICreditoDevolucaoResponse>
    {


        public CreditoDevolucaoHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }


        protected override async Task ValidateTransaction(TransactionCreditoDevolucao transaction, CancellationToken cancellationToken)
        {

            var _result = _validateService.ValidarChaveIdempotencia(transaction.chaveIdempotencia);
            if (!_result.IsValid)
                _validateException.erros.AddRange(_result.Errors);

            _result = _validateService.ValidarEndToEndIdOriginal(transaction.endToEndIdOriginal);
            if (!_result.IsValid)
                _validateException.erros.AddRange(_result.Errors);

            _result = _validateService.ValidarCodigoDevolucao(transaction.codigoDevolucao);
            if (!_result.IsValid)
                _validateException.erros.AddRange(_result.Errors);


            if (_validateException.erros.Count > 0)
                throw _validateException;

        }


        protected override async Task<JDPICreditoDevolucaoResponse> ExecuteTransactionProcessing(TransactionCreditoDevolucao transaction, CancellationToken cancellationToken)
        {
            try
            {
                var _result = await _spaRepoSql.RegistrarCreditoDevolucao(transaction);

                if (_result.exception is not null)
                    throw _result.exception;

                var _handleResult = await HandleProcessingResult(_result.result, _result.exception);

                return new JDPICreditoDevolucaoResponse(_handleResult);
            }
            catch (Exception dbEx)
            {
                _loggingAdapter.LogError("Database error", dbEx);
                throw;
            }
        }


        protected override BaseReturn<JDPICreditoDevolucaoResponse> ReturnSuccessResponse(JDPICreditoDevolucaoResponse result, string message, string correlationId)
        {
            return BaseReturn<JDPICreditoDevolucaoResponse>.FromSuccess(
                result,
                message,
                correlationId
            );
        }


        protected override BaseReturn<JDPICreditoDevolucaoResponse> ReturnErrorResponse(Exception exception, string correlationId)
        {
            return new BaseReturn<JDPICreditoDevolucaoResponse>(exception, false, correlationId);
        }

    }
}
