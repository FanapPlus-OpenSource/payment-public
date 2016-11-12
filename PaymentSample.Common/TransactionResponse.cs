namespace PaymentSample.Common
{
    public class TransactionResponse
    {
        public bool IsSuccess { get; set; }
        public string TransactionCode { get; set; }
        public AppsonErrorResponse Error { get; set; }
    }
}