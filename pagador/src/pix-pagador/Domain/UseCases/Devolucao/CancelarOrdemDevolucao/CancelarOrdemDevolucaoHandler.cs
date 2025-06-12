using Domain.Core.Base;
using Domain.Core.Models.Responses;
using Domain.UseCases.Pagamento.CancelarOrdemPagamento;

namespace Domain.UseCases.Devolucao.CancelarOrdemDevolucao
{
    public class CancelarOrdemDevolucaoHandler : BaseUseCaseHandler<TransactionCancelarOrdemDevolucao, BaseReturn<JDPICancelarOrdemDevolucaoResponse>, JDPICancelarOrdemDevolucaoResponse>
    {


        public CancelarOrdemDevolucaoHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            
        }


        protected override async Task ValidateTransaction(TransactionCancelarOrdemDevolucao transaction, CancellationToken cancellationToken)
        {
            var _result = _validateService.ValidarIdReqSistemaCliente(transaction.idReqSistemaCliente);
            if (!_result.IsValid)
                _validateException.erros.AddRange(_result.Errors);

            if (_validateException.erros.Count > 0)
                throw _validateException;

        }


        protected override async Task<JDPICancelarOrdemDevolucaoResponse> ExecuteTransactionProcessing(TransactionCancelarOrdemDevolucao transaction, CancellationToken cancellationToken)
        {
            try
            {
                var _result = await _spaRepoSql.CancelarOrdemDevolucao(transaction);
                var _handleResult = await HandleProcessingResult(_result.result, _result.exception);
                return new JDPICancelarOrdemDevolucaoResponse(_handleResult);

            }
            catch (Exception dbEx)
            {
                _loggingAdapter.LogError("Database error", dbEx);
                throw;
            }
        }


        protected override BaseReturn<JDPICancelarOrdemDevolucaoResponse> ReturnSuccessResponse(JDPICancelarOrdemDevolucaoResponse result, string message, string correlationId)
        {
            return BaseReturn<JDPICancelarOrdemDevolucaoResponse>.FromSuccess(
                result,
                message,
                correlationId
            );
        }


        protected override BaseReturn<JDPICancelarOrdemDevolucaoResponse> ReturnErrorResponse(Exception exception, string correlationId)
        {
            return new BaseReturn<JDPICancelarOrdemDevolucaoResponse>(exception, false, correlationId);
        }

    }
}
