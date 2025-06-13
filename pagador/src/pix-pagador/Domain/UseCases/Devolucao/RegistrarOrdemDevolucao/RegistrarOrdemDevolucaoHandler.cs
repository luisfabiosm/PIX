using Domain.Core.Base;
using Domain.Core.Models.Response;
using Domain.UseCases.Pagamento.RegistrarOrdemPagamento;

namespace Domain.UseCases.Devolucao.RegistrarOrdemDevolucao
{
    public class RegistrarOrdemDevolucaoHandler : BaseUseCaseHandler<TransactionRegistrarOrdemDevolucao, BaseReturn<JDPIRegistrarOrdemDevolucaoResponse>, JDPIRegistrarOrdemDevolucaoResponse>
    {
        public RegistrarOrdemDevolucaoHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        protected override async Task ValidateTransaction(TransactionRegistrarOrdemDevolucao transaction, CancellationToken cancellationToken)
        {
            var _result = _validateService.ValidarIdReqSistemaCliente(transaction.idReqSistemaCliente);
            if (!_result.IsValid)
                _validateException.erros.AddRange(_result.Errors);

            _result = _validateService.ValidarEndToEndIdOriginal(transaction.endToEndIdOriginal);
            if (!_result.IsValid)
                _validateException.erros.AddRange(_result.Errors);

  

            _result = _validateService.ValidarCodigoDevolucao(transaction.codigoDevolucao);
            if (!_result.IsValid)
                _validateException.erros.AddRange(_result.Errors);

            _result = _validateService.ValidarValor(transaction.valorDevolucao);
            if (!_result.IsValid)
                _validateException.erros.AddRange(_result.Errors);


            if (_validateException.erros.Count > 0)
                throw _validateException;

        }

        protected override async Task<JDPIRegistrarOrdemDevolucaoResponse> ExecuteTransactionProcessing(TransactionRegistrarOrdemDevolucao transaction, CancellationToken cancellationToken)
        {
            try
            {
                var _result = await _spaRepoSql.RegistrarOrdemDevolucao(transaction);
                var _handleResult = await HandleProcessingResult(_result.result, _result.exception);
                return new JDPIRegistrarOrdemDevolucaoResponse(_handleResult);

            }
            catch (Exception dbEx)
            {
                _loggingAdapter.LogError("Database error", dbEx);
                throw;
            }
        }


        protected override BaseReturn<JDPIRegistrarOrdemDevolucaoResponse> ReturnSuccessResponse(JDPIRegistrarOrdemDevolucaoResponse result, string message, string correlationId)
        {
            return BaseReturn<JDPIRegistrarOrdemDevolucaoResponse>.FromSuccess(
                result,
                message,
                correlationId
            );
        }


        protected override BaseReturn<JDPIRegistrarOrdemDevolucaoResponse> ReturnErrorResponse(Exception exception, string correlationId)
        {
            return new BaseReturn<JDPIRegistrarOrdemDevolucaoResponse>(exception, false, correlationId);
        }

    }
}
