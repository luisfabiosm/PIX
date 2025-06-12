

using Domain.Core.Mediator;
using System.Text.Json;

namespace Domain.Core.Base
{
    public abstract record BaseTransaction<TResponse> :  IBSRequest<TResponse>
    {
        public int Code { get; init; }

        public string CorrelationId { get; set; } = Guid.NewGuid().ToString();

        public int canal { get; set; }

        public string chaveIdempotencia { get; set; }


        public BaseTransaction()
        {
            
        }

        public abstract string getTransactionSerialization();
       

    }
}
