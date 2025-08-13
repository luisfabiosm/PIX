using Domain.Core.Common.Serialization;
using Domain.Core.Common.Transaction;
using Domain.Core.Models.Response.Error;

namespace Domain.Core.Models.Response;


public record ResponseSaldoPorTipoPapel : BaseTransactionResponse
{
    public List<ResultResponseSaldoPorTipoPapel> Result { get; set; }

    public ResponseSaldoPorTipoPapel(string result)
    {
        var _result = result.FromJsonOptimized<ResponseSaldoPorTipoPapel>(JsonOptions.Default);
        if (_result == null) return;

        Result = _result.Result;
    }
}


public record ResultResponseSaldoPorTipoPapel
{
    public string? cod_car { get; set; }
    public string? id_com_pap { get; set; }
    public string? Salatu { get; set; }
    public string? salbr { get; set; }
    public string? vr_rsg_min { get; set; }
    public string? vr_min_pmc { get; set; }
}