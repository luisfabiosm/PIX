using Domain.Core.Base;
using Domain.Core.Models.Responses;
using Domain.UseCases.Pagamento.RegistrarOrdemPagamento;

namespace Domain.UseCases.Pagamento.EfetivarOrdemPagamento
{
    public class EfetivarOrdemPagamentoHandler : BaseUseCaseHandler<TransactionEfetivarOrdemPagamento, BaseReturn<JDPIEfetivarOrdemPagamentoResponse>, JDPIEfetivarOrdemPagamentoResponse>
    {

        public EfetivarOrdemPagamentoHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            
        }

        protected override async Task ValidateTransaction(TransactionEfetivarOrdemPagamento transaction, CancellationToken cancellationToken)
        {
            var _result = _validateService.ValidarIdReqSistemaCliente(transaction.idReqSistemaCliente);
            if (!_result.IsValid)
                _validateException.erros.AddRange(_result.Errors);

           
            _result = _validateService.ValidarEndToEndId(transaction.endToEndId);
            if (!_result.IsValid)
                _validateException.erros.AddRange(_result.Errors);

            if (_validateException.erros.Count > 0)
                throw _validateException;

        }


        protected override async Task<JDPIEfetivarOrdemPagamentoResponse> ExecuteTransactionProcessing(TransactionEfetivarOrdemPagamento transaction, CancellationToken cancellationToken)
        {
            try
            {
                var _result = await _spaRepoSql.EfetivarOrdemPagamento(transaction);
                var _handleResult = await HandleProcessingResult(_result.result, _result.exception);
                return new JDPIEfetivarOrdemPagamentoResponse(_handleResult);

            }
            catch (Exception dbEx)
            {
                _loggingAdapter.LogError("Database error", dbEx);
                throw;
            }
        }


        protected override BaseReturn<JDPIEfetivarOrdemPagamentoResponse> ReturnSuccessResponse(JDPIEfetivarOrdemPagamentoResponse result, string message, string correlationId)
        {
            return BaseReturn<JDPIEfetivarOrdemPagamentoResponse>.FromSuccess(
                result,
                message,
                correlationId
            );
        }


        protected override BaseReturn<JDPIEfetivarOrdemPagamentoResponse> ReturnErrorResponse(Exception exception, string correlationId)
        {
            return new BaseReturn<JDPIEfetivarOrdemPagamentoResponse>(exception, false, correlationId);
        }

    }
}
