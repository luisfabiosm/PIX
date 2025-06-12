namespace Domain.Core.Base
{
    public record BaseTransactionResponse
    {
        public string CorrelationId { get; set; }

        public string chvAutorizador { get; set; }

    }
}
