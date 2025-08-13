using Domain.Core.Common.Serialization;
using Domain.Core.Common.Transaction;
using Domain.Core.Models.Response.Error;

namespace Domain.Core.Models.Response
{
    public record ResponseCarteira : BaseTransactionResponse
    {
        public List<ResultResponseCarteira> Result { get; set; }


        public ResponseCarteira(string result)
        {

            var _result = result.FromJsonOptimized<ResponseCarteira>(JsonOptions.Default);
            if (_result == null) return;

            Result = _result.Result;
        }
    }
    public record ResultResponseCarteira
    {
        public int cod_car { get; set; }
        public string? nom_car { get; set; }
    }
}