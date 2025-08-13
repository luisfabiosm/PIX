using Domain.Core.Common.ResultPattern;
using Domain.Core.Common.Serialization;
using Domain.Core.Common.Transaction;


namespace Domain.Core.Models.Response
{
    public record ResponsePapelDispAplic : BaseTransactionResponse
    {
        public List<ResultPapelDispAplic> Result { get; set; }


        public ResponsePapelDispAplic(string result)
        {
            var _result = result.FromJsonOptimized<ResponsePapelDispAplic>(JsonOptions.Default);
            if (_result == null) return;

            Result = _result.Result;
        }
    }


    public record ResultPapelDispAplic
    {
        public string cod_car { get; set; }
        public string id_com_pap { get; set; }
        public string nom_car { get; set; }
        public string vr_min { get; set; }
        public string vr_min_adi { get; set; }
        public string vr_rsg_min { get; set; }
        public string vr_min_pmc { get; set; }
        public string sldcli { get; set; }
        public string pubasemi { get; set; }

       
    }
}