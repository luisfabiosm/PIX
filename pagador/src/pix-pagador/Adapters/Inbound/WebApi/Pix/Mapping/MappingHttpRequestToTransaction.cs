
using Domain.Core.Enum;
using Domain.Core.Models.Request;
using Domain.UseCases.Pagamento.CancelarOrdemPagamento;
using Domain.UseCases.Pagamento.EfetivarOrdemPagamento;
using Domain.UseCases.Pagamento.RegistrarOrdemPagamento;
using System.Linq.Expressions;

namespace Adapters.Inbound.WebApi.Pix.Mapping
{
    public class MappingHttpRequestToTransaction
    {
        public TransactionRegistrarOrdemPagamento ToTransactionRegistrarOrdemPagamento(JDPIRegistrarOrdemPagtoRequest request, string correlationId, int code)
        {
            try
            {
                return new TransactionRegistrarOrdemPagamento
                {
                    idReqSistemaCliente = request.idReqSistemaCliente,
                    CorrelationId = correlationId,
                    Code = code,
                    pagador = request.pagador,
                    recebedor = request.recebedor,
                    tpIniciacao = request.tpIniciacao,
                    valor = request.valor,
                    infEntreClientes = request.infEntreClientes,
                    prioridadePagamento = request.prioridadePagamento,
                    tpPrioridadePagamento = request.tpPrioridadePagamento,
                    finalidade = request.finalidade,
                    modalidadeAgente = request.modalidadeAgente,
                    ispbPss = request.ispbPss,
                    cnpjIniciadorPagamento = request.cnpjIniciadorPagamento,
                    vlrDetalhe = request.vlrDetalhe,
                    agendamentoID = request.agendamentoID,
                    qrCode = request.qrCode,
                    consentId = request.consentId,
                    idConciliacaoRecebedor = request.idConciliacaoRecebedor,
                    chave = request.chave,

                };
            }
            catch (Exception)
            {
                throw;
            }
        }



        public TransactionEfetivarOrdemPagamento ToTransactionEfetivarOrdemPagamento(JDPIEfetivarOrdemPagtoRequest request, string correlationId, int code)
        {
            try
            {
                return new TransactionEfetivarOrdemPagamento
                {
                    idReqSistemaCliente = request.idReqSistemaCliente,
                    CorrelationId = correlationId,
                    Code = code,
                    agendamentoID = request.agendamentoID,
                    idReqJdPi = request.idReqJdPi,
                    endToEndId = request.endToEndId,
                    dtHrReqJdPi = request.dtHrReqJdPi
                };
            }
            catch (Exception)
            {
                throw;
            }
        }


        public TransactionCancelarOrdemPagamento ToTransactionCancelarOrdemPagamento(JDPICancelarRegistroOrdemPagtoRequest request, string correlationId, int code)
        {
            try
            {
                return new TransactionCancelarOrdemPagamento
                {
                    CorrelationId = correlationId,
                    Code = code,
                    idReqSistemaCliente = request.idReqSistemaCliente,
                    agendamentoID = request.agendamentoID,
                    motivo = request.motivo,
                    tipoErro = request.tipoErro
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}

      