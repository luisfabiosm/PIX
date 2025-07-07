using Domain.Core.Models.Request;
using Domain.Services;
using Domain.UseCases.Devolucao.CancelarOrdemDevolucao;
using Domain.UseCases.Devolucao.EfetivarOrdemDevolucao;
using Domain.UseCases.Devolucao.RegistrarOrdemDevolucao;
using Domain.UseCases.Pagamento.CancelarOrdemPagamento;
using Domain.UseCases.Pagamento.EfetivarOrdemPagamento;
using Domain.UseCases.Pagamento.RegistrarOrdemPagamento;

namespace Adapters.Inbound.WebApi.Pix.Mapping
{


    public class MappingHttpRequestToTransaction
    {
        private readonly ContextAccessorService _contextAccessor;
        private short Canal(HttpContext context) => _contextAccessor.GetCanal(context);
        private string Idempotencia(HttpContext context) => _contextAccessor.GetChaveIdempotencia(context);

        public MappingHttpRequestToTransaction(IServiceProvider serviceProvider)
        {
            _contextAccessor = serviceProvider.GetRequiredService<ContextAccessorService>();
        }

        public TransactionRegistrarOrdemPagamento ToTransactionRegistrarOrdemPagamento(HttpContext context, JDPIRegistrarOrdemPagtoRequest request, string correlationId, int code)
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
                    endToEndId = request.endToEndId,
                    dtEnvioPag = request.dtEnvioPag,
                    consentId = request.consentId,
                    idConciliacaoRecebedor = request.idConciliacaoRecebedor,
                    chave = request.chave,
                    canal = Canal(context),
                    chaveIdempotencia = Idempotencia(context)
                };
            }
            catch (Exception)
            {
                throw;
            }
        }



        public TransactionEfetivarOrdemPagamento ToTransactionEfetivarOrdemPagamento(HttpContext context, JDPIEfetivarOrdemPagtoRequest request, string correlationId, int code)
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
                    dtHrReqJdPi = request.dtHrReqJdPi,
                    canal = Canal(context),
                    chaveIdempotencia = Idempotencia(context)
                };
            }
            catch (Exception)
            {
                throw;
            }
        }


        public TransactionCancelarOrdemPagamento ToTransactionCancelarOrdemPagamento(HttpContext context, JDPICancelarRegistroOrdemPagtoRequest request, string correlationId, int code)
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
                    tipoErro = request.tipoErro,
                    canal = Canal(context),
                    chaveIdempotencia = Idempotencia(context)
                };
            }
            catch (Exception)
            {
                throw;
            }
        }


        public TransactionRegistrarOrdemDevolucao ToTransactionRegistrarOrdemDevolucao(HttpContext context, JDPIRequisitarDevolucaoOrdemPagtoRequest request, string correlationId, int code)
        {
            try
            {

                return new TransactionRegistrarOrdemDevolucao
                {
                    CorrelationId = correlationId,
                    Code = code,
                    idReqSistemaCliente = request.idReqSistemaCliente,
                    endToEndIdOriginal = request.endToEndIdOriginal,
                    endToEndIdDevolucao = request.endToEndIdDevolucao,
                    codigoDevolucao = request.codigoDevolucao,
                    motivoDevolucao = request.motivoDevolucao,
                    valorDevolucao = request.valorDevolucao,
                    canal = Canal(context),
                    chaveIdempotencia = Idempotencia(context)


                };
            }
            catch (Exception)
            {
                throw;
            }
        }


        public TransactionEfetivarOrdemDevolucao ToTransactionEfetivarOrdemDevolucao(HttpContext context, JDPIEfetivarOrdemDevolucaoRequest request, string correlationId, int code)
        {
            try
            {

                return new TransactionEfetivarOrdemDevolucao
                {
                    CorrelationId = correlationId,
                    Code = code,
                    idReqSistemaCliente = request.idReqSistemaCliente,
                    idReqJdPi = request.idReqJdPi,
                    endToEndIdOriginal = request.endToEndIdOriginal,
                    endToEndIdDevolucao = request.endToEndIdDevolucao,
                    dtHrReqJdPi = request.dtHrReqJdPi,
                    canal = Canal(context),
                    chaveIdempotencia = Idempotencia(context)

                };

            }
            catch (Exception)
            {
                throw;
            }
        }


        public TransactionCancelarOrdemDevolucao ToTransactionCancelarOrdemDevolucao(HttpContext context, JDPICancelarRegistroOrdemdDevolucaoRequest request, string correlationId, int code)
        {
            try
            {

                return new TransactionCancelarOrdemDevolucao
                {
                    CorrelationId = correlationId,
                    Code = code,
                    idReqSistemaCliente = request.idReqSistemaCliente,
                    canal = Canal(context),
                    chaveIdempotencia = Idempotencia(context)

                };

            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}

