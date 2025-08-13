using Domain.Core.Common.Serialization;
using Domain.Core.Common.Transaction;
using Domain.Core.Models.Response.Error;

namespace Domain.Core.Models.Response
{
    public record ResponseAplicacaoNoDia : BaseTransactionResponse
    {
        public List<ResultResponseAplicacaoNoDia> Result { get; set; }


        public ResponseAplicacaoNoDia(string result)
        {
            var _result = result.FromJsonOptimized<ResponseAplicacaoNoDia>(JsonOptions.Default);
            if (_result == null) return;

            Result = _result.Result;
        }
    }
    public record ResultResponseAplicacaoNoDia
    { 
        public int cod_car { get; set; }
        public string? id_com_pap { get; set; }
        public string? Numest { get; set; }
        public decimal VrFinIda { get; set; }
        public DateTime Dtapl { get; set; }
        public DateTime Dtvcto { get; set; }
        public int NSUAut { get; set; }
    }
}
