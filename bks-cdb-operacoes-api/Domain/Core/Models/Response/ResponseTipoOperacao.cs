using Domain.Core.Common.ResultPattern;
using Domain.Core.Common.Serialization;
using Domain.Core.Common.Transaction;
using Domain.Core.Models.Response.Error;

namespace Domain.Core.Models.Response
{
    public record ResponseTipoOperacao : BaseTransactionResponse
    {
        public List<ResultResponseTipoOperacao> Result { get; set; }

        public ResponseTipoOperacao(string result)
        {
            var _result = result.FromJsonOptimized<ResponseTipoOperacao>(JsonOptions.Default);
            if (_result == null) return;

            Result = _result.Result;
        }
    }

    public record ResultResponseTipoOperacao
    {
        public string? Id_ope { get; set; }
        public string? Nom_ope { get; set; }
    }
}