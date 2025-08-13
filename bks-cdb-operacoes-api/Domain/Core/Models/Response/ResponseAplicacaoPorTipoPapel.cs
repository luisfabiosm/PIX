using Domain.Core.Common.Serialization;
using Domain.Core.Common.Transaction;
using Domain.Core.Models.Response.Error;

namespace Domain.Core.Models.Response
{
    public record ResponseAplicacaoPorTipoPapel : BaseTransactionResponse
    {
        public List<ResultResponseAplicacaoPorTipoPapel> Result { get; set; }


        public ResponseAplicacaoPorTipoPapel(string result)
        {
            var _result = result.FromJsonOptimized<ResponseAplicacaoPorTipoPapel>(JsonOptions.Default);
            if (_result == null) return;

            Result = _result.Result;
        }
    }
    public record ResultResponseAplicacaoPorTipoPapel
    {
        public int cod_car { get; set; }
        public string? id_com_pap { get; set; }
        public int NumOpe { get; set; }
        public int Numest { get; set; }
        public decimal Salatu { get; set; }
        public DateTime Dtapl { get; set; }
        public DateTime Dtvcto { get; set; }
    }
}
