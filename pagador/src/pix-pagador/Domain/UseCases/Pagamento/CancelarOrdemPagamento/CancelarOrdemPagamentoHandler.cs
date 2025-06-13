using Domain.Core.Base;
using Domain.Core.Models.Response;
using Domain.UseCases.Pagamento.EfetivarOrdemPagamento;

namespace Domain.UseCases.Pagamento.CancelarOrdemPagamento
{
    public class CancelarOrdemPagamentoHandler : BaseUseCaseHandler<TransactionCancelarOrdemPagamento, BaseReturn<JDPICancelarOrdemPagamentoResponse>, JDPICancelarOrdemPagamentoResponse>
    {

        public CancelarOrdemPagamentoHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        protected override async Task ValidateTransaction(TransactionCancelarOrdemPagamento transaction, CancellationToken cancellationToken)
        {
            var _result = _validateService.ValidarIdReqSistemaCliente(transaction.idReqSistemaCliente);
            if (!_result.IsValid)
                _validateException.erros.AddRange(_result.Errors);


            _result = _validateService.ValidarMotivo(transaction.motivo);
            if (!_result.IsValid)
                _validateException.erros.AddRange(_result.Errors);

            if (_validateException.erros.Count > 0)
                throw _validateException;

        }


        protected override async Task<JDPICancelarOrdemPagamentoResponse> ExecuteTransactionProcessing(TransactionCancelarOrdemPagamento transaction, CancellationToken cancellationToken)
        {
            try
            {
                var _result = await _spaRepoSql.CancelarOrdemPagamento(transaction);
                var _handleResult = await HandleProcessingResult(_result.result, _result.exception);
                return new JDPICancelarOrdemPagamentoResponse(_handleResult);

            }
            catch (Exception dbEx)
            {
                _loggingAdapter.LogError("Database error", dbEx);
                throw;
            }
        }


        protected override BaseReturn<JDPICancelarOrdemPagamentoResponse> ReturnSuccessResponse(JDPICancelarOrdemPagamentoResponse result, string message, string correlationId)
        {
            return BaseReturn<JDPICancelarOrdemPagamentoResponse>.FromSuccess(
                result,
                message,
                correlationId
            );
        }


        protected override BaseReturn<JDPICancelarOrdemPagamentoResponse> ReturnErrorResponse(Exception exception, string correlationId)
        {
            return new BaseReturn<JDPICancelarOrdemPagamentoResponse>(exception, false, correlationId);
        }

    }
}
