using Domain.Core.Enum;
using Domain.Core.Exceptions;
using Domain.Core.Models.JDPI;

namespace Domain.Core.Ports.Domain
{
    public interface IValidatorService
    {
        (List<ValidationErrorDetails> Errors, bool IsValid) ValidarPagador(JDPIDadosConta pagador);

        (List<ValidationErrorDetails> Errors, bool IsValid) ValidarRecebedor(JDPIDadosConta pagador);

        (List<ValidationErrorDetails> Errors, bool IsValid) ValidardtHrOp(string dtHrOp);


        (List<ValidationErrorDetails> Errors, bool IsValid) ValidarMotivo(string motivo);


        (List<ValidationErrorDetails> Errors, bool IsValid) ValidarValor(double valor);

        (List<ValidationErrorDetails> Errors, bool IsValid) ValidarChaveIdempotencia(string chaveIdempotencia);

        (List<ValidationErrorDetails> Errors, bool IsValid) ValidarEndToEndIdOriginal(string endToEndIdOriginal);

        (List<ValidationErrorDetails> Errors, bool IsValid) ValidarEndToEndId(string endToEndId);

        (List<ValidationErrorDetails> Errors, bool IsValid) ValidarCodigoDevolucao(string codigoDevolucao);

        (List<ValidationErrorDetails> Errors, bool IsValid) ValidarIdReqSistemaCliente(string idReqSistemaCliente);

        (List<ValidationErrorDetails> Errors, bool IsValid) ValidarTpIniciacao(EnumTpIniciacao tpIniciacao);

        (List<ValidationErrorDetails> Errors, bool IsValid) ValidarPrioridadePagamento(EnumPrioridadePagamento? prioridadePagamento);

        (List<ValidationErrorDetails> Errors, bool IsValid) ValidarTipoPrioridadePagamento(EnumTpPrioridadePagamento? tpPrioridadePagamento);

        (List<ValidationErrorDetails> Errors, bool IsValid) ValidarFinalidade(EnumTipoFinalidade? finalidade);

        (List<ValidationErrorDetails> Errors, bool IsValid) ValidarValorDevolucao(double valorDevolucao);


    }
}
