using Domain.Core.Exceptions;
using Domain.Core.Models.JDPI;

namespace Domain.Core.Interfaces.Domain
{
    public interface IValidatorService
    {
        (List<ErrorDetails> Errors, bool IsValid) ValidarPagador(JDPIDadosContaPagador pagador);

        (List<ErrorDetails> Errors, bool IsValid) ValidarRecebedor(JDPIDadosContaRecebedor pagador);

        (List<ErrorDetails> Errors, bool IsValid) ValidardtHrOp(string dtHrOp);

        (List<ErrorDetails> Errors, bool IsValid) ValidarValor(double valor);

        (List<ErrorDetails> Errors, bool IsValid) ValidarChaveIdempotencia(string chaveIdempotencia);

        (List<ErrorDetails> Errors, bool IsValid) ValidarEndToEndIdOriginal(string endToEndIdOriginal);

        (List<ErrorDetails> Errors, bool IsValid) ValidarCodigoDevolucao(string codigoDevolucao);
    }
}
