using Domain.Core.Base;
using Domain.Core.Models.Responses;
using Domain.UseCases.ValidarContaCliente;

namespace Domain.UseCases.CreditoOrdemPagamento
{
    public class CreditoOrdemPagamentoHandler : BaseUseCaseHandler<TransactionCreditoOrdemPagamento, BaseReturn<JDPICreditoOrdemPagamentoResponse>, JDPICreditoOrdemPagamentoResponse>
    {

     
        public CreditoOrdemPagamentoHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            
        }


        protected override async Task ValidateTransaction(TransactionCreditoOrdemPagamento transaction, CancellationToken cancellationToken)
        {


            var _result = _validateService.ValidarPagador(transaction.pagador);
            if (!_result.IsValid)
                _validateException.erros.AddRange(_result.Errors);

            _result = _validateService.ValidarRecebedor(transaction.recebedor);
            if (!_result.IsValid)
                _validateException.erros.AddRange(_result.Errors);

            _result = _validateService.ValidarChaveIdempotencia(transaction.chaveIdempotencia);
            if (!_result.IsValid)
                _validateException.erros.AddRange(_result.Errors);

            _result = _validateService.ValidardtHrOp(transaction.dtHrOp);
            if (!_result.IsValid)
                _validateException.erros.AddRange(_result.Errors);

            _result = _validateService.ValidarValor(transaction.valor);
            if (!_result.IsValid)
                _validateException.erros.AddRange(_result.Errors);

            if (_validateException.erros.Count > 0)
                throw _validateException;

        }


        protected override async Task<JDPICreditoOrdemPagamentoResponse> ExecuteTransactionProcessing(TransactionCreditoOrdemPagamento transaction, CancellationToken cancellationToken)
        {
            try
            {
                var _result = await _spaRepoSql.RegistrarCreditoOrdemPagamento(transaction);

                var _handleResult = await HandleProcessingResult(_result.result, _result.exception);

                return new JDPICreditoOrdemPagamentoResponse(_handleResult);
            }
            catch (Exception dbEx)
            {
                _loggingAdapter.LogError("Database error", dbEx);
                throw;
            }
        }


        protected override BaseReturn<JDPICreditoOrdemPagamentoResponse> ReturnSuccessResponse(JDPICreditoOrdemPagamentoResponse result, string message, string correlationId)
        {
            return BaseReturn<JDPICreditoOrdemPagamentoResponse>.FromSuccess(
                result,
                message,
                correlationId
            );
        }


        protected override BaseReturn<JDPICreditoOrdemPagamentoResponse> ReturnErrorResponse(Exception exception, string correlationId)
        {
            return new BaseReturn<JDPICreditoOrdemPagamentoResponse>(exception, false, correlationId);
        }


    }
}
