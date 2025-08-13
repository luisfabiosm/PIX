using Domain.Core.Common.ResultPattern;
using Domain.Core.Common.Serialization;
using Domain.Core.Common.Transaction;
using Domain.Core.Models.Response.Error;

namespace Domain.Core.Models.Response
{
    public record ResponsePapeisPorCarteira : BaseTransactionResponse
    {
        public List<ResultResponsePapeisPorCarteira> Result { get; set; }

        public ResponsePapeisPorCarteira(string result)
        {

            var _result = result.FromJsonOptimized<ResponsePapeisPorCarteira>(JsonOptions.Default);
            if (_result == null) return;

            Result = _result.Result;
        }
        public record ResultResponsePapeisPorCarteira
        {
            public string? id_com_pap { get; set; }
        }
    }
}