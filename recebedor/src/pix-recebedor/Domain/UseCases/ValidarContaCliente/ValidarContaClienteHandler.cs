using Domain.Core.Base;
using Domain.Core.Exceptions;
using Domain.Core.Interfaces.Domain;
using Domain.Core.Models.Responses;
using System.Collections.Generic;


namespace Domain.UseCases.ValidarContaCliente
{
    public class ValidarContaClienteHandler : BaseUseCaseHandler<TransactionValidarContaCliente, BaseReturn<JDPIValidarContaClienteResponse>, JDPIValidarContaClienteResponse>
    {
   
        public ValidarContaClienteHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
         
        }


        protected override async Task ValidateTransaction(TransactionValidarContaCliente transaction, CancellationToken cancellationToken)
        {
            var _result = _validateService.ValidarPagador(transaction.pagador);
            if (!_result.IsValid)
                _validateException.erros.AddRange(_result.Errors);

            _result = _validateService.ValidarRecebedor(transaction.recebedor);
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


        protected override async Task<JDPIValidarContaClienteResponse> ExecuteTransactionProcessing(TransactionValidarContaCliente transaction, CancellationToken cancellationToken)
        {
            try
            {
                var _result = await _spaRepoSql.ValidarConta(transaction);
                var _handleResult = await HandleProcessingResult(_result.result, _result.exception);
                return new JDPIValidarContaClienteResponse(_handleResult);

            }
            catch (Exception dbEx)
            {
                _loggingAdapter.LogError("Database error", dbEx);
                throw;
            }
        }


        protected override BaseReturn<JDPIValidarContaClienteResponse> ReturnSuccessResponse(JDPIValidarContaClienteResponse result, string message, string correlationId)
        {
            return BaseReturn<JDPIValidarContaClienteResponse>.FromSuccess(
                result,
                message,
                correlationId
            );
        }


        protected override BaseReturn<JDPIValidarContaClienteResponse> ReturnErrorResponse(Exception exception, string correlationId)
        {
            return new BaseReturn<JDPIValidarContaClienteResponse>(exception, false, correlationId);
        }


        

    }
}
