using Domain.Core.Enums;
using Domain.Core.Exceptions;
using Domain.Core.Interfaces.Domain;
using Domain.Core.Models.JDPI;

namespace Domain.Services
{
    public class ValidatorService : IValidatorService
    {


        public (List<ErrorDetails> Errors, bool IsValid) ValidarPagador(JDPIDadosContaPagador pagador)
        {
            var _errors = new List<ErrorDetails>();

            if (pagador is null)
            {
                _errors.Add(new ErrorDetails("pagador", "pagador deve ser preenchido"));
                return (_errors, false);
            }

            ValidateRequired(pagador.ispb, "pagador.ispb", _errors);
            ValidateRequired(pagador.cpfCnpj, "pagador.cpfCnpj", _errors);
            ValidateRequired(pagador.nome, "pagador.nome", _errors);

            ValidateEnum(pagador.tpPessoa, typeof(EnumTipoPessoa), "pagador.tpPessoa", _errors);
            ValidateEnum(pagador.tpConta, typeof(EnumTipoConta), "pagador.tpConta", _errors);

            return (_errors, _errors.Count == 0);
        }


        public (List<ErrorDetails> Errors, bool IsValid) ValidarRecebedor(JDPIDadosContaRecebedor recebedor)
        {
            var errors = new List<ErrorDetails>();

            if (recebedor is null)
            {
                errors.Add(new ErrorDetails("recebedor", "recebedor deve ser preenchido"));
                return (errors, false);
            }

            ValidateRequired(recebedor.ispb, "recebedor.ispb", errors);
            ValidateRequired(recebedor.cpfCnpj, "recebedor.cpfCnpj", errors);

            ValidateEnum(recebedor.tpPessoa, typeof(EnumTipoPessoa), "recebedor.tpPessoa", errors);
            ValidateEnum(recebedor.tpConta, typeof(EnumTipoConta), "recebedor.tpConta", errors);

            return (errors, errors.Count == 0);
        }


        public (List<ErrorDetails> Errors, bool IsValid) ValidardtHrOp(string dtHrOp)
        {
            var errors = new List<ErrorDetails>();

            ValidateRequired(dtHrOp, "dtHrOp", errors);

            return (errors, errors.Count == 0);
        }


        public (List<ErrorDetails> Errors, bool IsValid) ValidarValor(double valor)
        {
            var errors = new List<ErrorDetails>();

            ValidateRequired(valor, "valor", errors);

            return (errors, errors.Count == 0);
        }

        public (List<ErrorDetails> Errors, bool IsValid) ValidarChaveIdempotencia(string chaveIdempotencia)
        {
            var errors = new List<ErrorDetails>();

            ValidateRequired(chaveIdempotencia, "chaveIdempotencia", errors);

            return (errors, errors.Count == 0);
        }

        public (List<ErrorDetails> Errors, bool IsValid) ValidarEndToEndIdOriginal(string endToEndIdOriginal)
        {
            var errors = new List<ErrorDetails>();

            ValidateRequired(endToEndIdOriginal, "endToEndIdOriginal", errors);

            return (errors, errors.Count == 0);
        }

        public (List<ErrorDetails> Errors, bool IsValid) ValidarCodigoDevolucao(string codigoDevolucao)
        {
            var errors = new List<ErrorDetails>();

            ValidateRequired(codigoDevolucao, "codigoDevolucao", errors);

            return (errors, errors.Count == 0);
        }



        private void ValidateRequired<T>(T value, string fieldName, List<ErrorDetails> errors)
        {
            if (value == null || value.Equals(default(T)) || (value is string s && string.IsNullOrWhiteSpace(s)))
            {
                errors.Add(new ErrorDetails( fieldName, $"{fieldName} deve ser informado e nao pode ser nulo"));
            }
        }

        private void ValidateEnum(object value, Type enumType, string fieldName, List<ErrorDetails> errors)
        {
            if (value == null)
            {
                errors.Add(new ErrorDetails(fieldName, $"{fieldName} deve ser informado e nao pode ser nulo"));
            }
            else if (!Enum.IsDefined(enumType, value))
            {
                errors.Add(new ErrorDetails(fieldName, $"{fieldName} deve ser preenchido com domínio válido"));
            }
        }

    }
}
