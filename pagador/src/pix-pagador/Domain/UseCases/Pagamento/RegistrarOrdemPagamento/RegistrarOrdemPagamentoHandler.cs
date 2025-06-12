using Domain.Core.Base;
using Domain.Core.Models.Responses;

namespace Domain.UseCases.Pagamento.RegistrarOrdemPagamento
{
    public class RegistrarOrdemPagamentoHandler : BaseUseCaseHandler<TransactionRegistrarOrdemPagamento, BaseReturn<JDPIRegistrarOrdemPagamentoResponse>, JDPIRegistrarOrdemPagamentoResponse>
    {

        public RegistrarOrdemPagamentoHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            
        }


        protected override async Task ValidateTransaction(TransactionRegistrarOrdemPagamento transaction, CancellationToken cancellationToken)
        {
            var _result = _validateService.ValidarIdReqSistemaCliente(transaction.idReqSistemaCliente);
            if (!_result.IsValid)
                _validateException.erros.AddRange(_result.Errors);

            _result = _validateService.ValidarTpIniciacao(transaction.tpIniciacao);
            if (!_result.IsValid)
                _validateException.erros.AddRange(_result.Errors);

            _result = _validateService.ValidarPagador(transaction.pagador);
            if (!_result.IsValid)
                _validateException.erros.AddRange(_result.Errors);

            _result = _validateService.ValidarRecebedor(transaction.recebedor);
            if (!_result.IsValid)
                _validateException.erros.AddRange(_result.Errors);

            _result = _validateService.ValidarValor(transaction.valor);
            if (!_result.IsValid)
                _validateException.erros.AddRange(_result.Errors);

            _result = _validateService.ValidarChaveIdempotencia(transaction.chaveIdempotencia);
            if (!_result.IsValid)
                _validateException.erros.AddRange(_result.Errors);

            _result = _validateService.ValidarPrioridadePagamento(transaction.prioridadePagamento);
            if (!_result.IsValid)
                _validateException.erros.AddRange(_result.Errors);

            _result = _validateService.ValidarTipoPrioridadePagamento(transaction.tpPrioridadePagamento);
            if (!_result.IsValid)
                _validateException.erros.AddRange(_result.Errors);

            _result = _validateService.ValidarTipoPrioridadePagamento(transaction.tpPrioridadePagamento);
            if (!_result.IsValid)
                _validateException.erros.AddRange(_result.Errors);

            _result = _validateService.ValidarFinalidade(transaction.finalidade);
            if (!_result.IsValid)
                _validateException.erros.AddRange(_result.Errors);


            if (_validateException.erros.Count > 0)
                throw _validateException;

        }


        protected override async Task<JDPIRegistrarOrdemPagamentoResponse> ExecuteTransactionProcessing(TransactionRegistrarOrdemPagamento transaction, CancellationToken cancellationToken)
        {
            try
            {
                var _result = await _spaRepoSql.RegistrarOrdemPagamento(transaction);
                var _handleResult = await HandleProcessingResult(_result.result, _result.exception);
                return new JDPIRegistrarOrdemPagamentoResponse(_handleResult);

            }
            catch (Exception dbEx)
            {
                _loggingAdapter.LogError("Database error", dbEx);
                throw;
            }
        }


        protected override BaseReturn<JDPIRegistrarOrdemPagamentoResponse> ReturnSuccessResponse(JDPIRegistrarOrdemPagamentoResponse result, string message, string correlationId)
        {
            return BaseReturn<JDPIRegistrarOrdemPagamentoResponse>.FromSuccess(
                result,
                message,
                correlationId
            );
        }


        protected override BaseReturn<JDPIRegistrarOrdemPagamentoResponse> ReturnErrorResponse(Exception exception, string correlationId)
        {
            return new BaseReturn<JDPIRegistrarOrdemPagamentoResponse>(exception, false, correlationId);
        }


    }
}
