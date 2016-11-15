using PaymentSample.Common;

namespace PaymentSample.Services
{
    public class ChargeRequestDto : RequestDto
    {
        public string ProductItemCode { get; set; }
        public string ReferenceId { get; set; }
        public string Token { get; set; }
    }
}