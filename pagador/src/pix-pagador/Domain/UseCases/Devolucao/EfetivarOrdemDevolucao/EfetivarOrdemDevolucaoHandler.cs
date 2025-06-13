using Domain.Core.Base;
using Domain.Core.Models.Request;
using Domain.Core.Models.Response;
using Domain.UseCases.Devolucao.RegistrarOrdemDevolucao;

namespace Domain.UseCases.Devolucao.EfetivarOrdemDevolucao
{
    public class EfetivarOrdemDevolucaoHandler : BaseUseCaseHandler<TransactionEfetivarOrdemDevolucao, BaseReturn<JDPIEfetivarOrdemDevolucaoResponse>, JDPIEfetivarOrdemDevolucaoResponse>
    {


        public EfetivarOrdemDevolucaoHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        protected override async Task ValidateTransaction(TransactionEfetivarOrdemDevolucao transaction, CancellationToken cancellationToken)
        {
            var _result = _validateService.ValidarIdReqSistemaCliente(transaction.idReqSistemaCliente);
            if (!_result.IsValid)
                _validateException.erros.AddRange(_result.Errors);


            _result = _validateService.ValidarEndToEndId(transaction.endToEndIdOriginal);
            if (!_result.IsValid)
                _validateException.erros.AddRange(_result.Errors);


            if (_validateException.erros.Count > 0)
                throw _validateException;

        }

        protected override async Task<JDPIEfetivarOrdemDevolucaoResponse> ExecuteTransactionProcessing(TransactionEfetivarOrdemDevolucao transaction, CancellationToken cancellationToken)
        {
            try
            {
                var _result = await _spaRepoSql.EfetivarOrdemDevolucao(transaction);
                var _handleResult = await HandleProcessingResult(_result.result, _result.exception);
                return new JDPIEfetivarOrdemDevolucaoResponse(_handleResult);

            }
            catch (Exception dbEx)
            {
                _loggingAdapter.LogError("Database error", dbEx);
                throw;
            }
        }


        protected override BaseReturn<JDPIEfetivarOrdemDevolucaoResponse> ReturnSuccessResponse(JDPIEfetivarOrdemDevolucaoResponse result, string message, string correlationId)
        {
            return BaseReturn<JDPIEfetivarOrdemDevolucaoResponse>.FromSuccess(
                result,
                message,
                correlationId
            );
        }


        protected override BaseReturn<JDPIEfetivarOrdemDevolucaoResponse> ReturnErrorResponse(Exception exception, string correlationId)
        {
            return new BaseReturn<JDPIEfetivarOrdemDevolucaoResponse>(exception, false, correlationId);
        }


    }
}
