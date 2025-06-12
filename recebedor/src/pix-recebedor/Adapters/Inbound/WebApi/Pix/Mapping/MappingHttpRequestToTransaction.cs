
using Domain.Core.Models.Requests;
using Domain.UseCases.CreditoDevolucao;
using Domain.UseCases.CreditoOrdemPagamento;
using Domain.UseCases.ValidarContaCliente;
using System.Linq.Expressions;

namespace Adapters.Inbound.WebApi.Pix.Mapping
{
    public class MappingHttpRequestToTransaction
    {
        public TransactionValidarContaCliente ToTransactionValidarContaCliente(JDPIValidarContaRequest request, string correlationId, int code)
        {
            try
            {
                return new TransactionValidarContaCliente
                {
                    CorrelationId = correlationId,
                    Code = code,
                    pagador = request.pagador,
                    recebedor = request.recebedor,
                    dtHrOp = request.dtHrOp,
                    valor = request.valor,
                    infEntreClientes = request.infEntreClientes,
                    creditoOrdemPagamento = request.creditoOrdemPagamento,
                    creditoDevolucao = request.creditoDevolucao,
                    tpIniciacao = request.tpIniciacao,
                    prioridadePagamento = request.prioridadePagamento,
                    tpPrioridadePagamento = request.tpPrioridadePagamento,
                    finalidade = request.finalidade,
                    modalidadeAgente = request.modalidadeAgente,
                    ispbPss = request.ispbPss,
                    cnpjIniciadorPagamento = request.cnpjIniciadorPagamento,
                    vlrDetalhe = request.vlrDetalhe
                };
            }
            catch (Exception)
            {
                throw;
            }
        }


        public TransactionCreditoOrdemPagamento ToTransactionCreditoOrdemPagamento(JDPICreditoOrdemPagamentoRequest request, string correlationId, int code)
        {
            try
            {
                //rever
                return new TransactionCreditoOrdemPagamento
                {
                    CorrelationId = correlationId,
                    Code = code,
                    pagador = request.pagador,
                    recebedor = request.recebedor,
                    dtHrOp = request.dtHrOp,
                    valor = request.valor,
                    infEntreClientes = request.infEntreClientes,
                    tpIniciacao = request.tpIniciacao,
                    prioridadePagamento = request.prioridadePagamento,
                    tpPrioridadePagamento = request.tpPrioridadePagamento,
                    finalidade = request.finalidade,
                    modalidadeAgente = request.modalidadeAgente,
                    ispbPss = request.ispbPss,
                    cnpjIniciadorPagamento = request.cnpjIniciadorPagamento,
                    vlrDetalhe = request.vlrDetalhe
                };
            }
            catch (Exception)
            {
                throw;
            }
        }


        public TransactionCreditoDevolucao ToTransactionCreditoDevolucao(JDPICreditoDevolucaoRequest request, string correlationId, int code)
        {
            try
            {
                //rever
                return new TransactionCreditoDevolucao
                {
                    CorrelationId = correlationId,
                    Code = code,
                    pagador = request.pagador,
                    recebedor = request.recebedor,
                    dtHrOp = request.dtHrOp,
                    valor = request.valor,
                    infEntreClientes = request.infEntreClientes,
                    endToEndIdOriginal = request.endToEndIdOriginal,
                    endToEndIdDevolucao = request.endToEndIdDevolucao,
                    codigoDevolucao = request.codigoDevolucao,
                    idReqJdPi = request.idReqJdPi,
                };
            }
            catch (Exception)
            {
                throw;
            }
        }



    }
}

      