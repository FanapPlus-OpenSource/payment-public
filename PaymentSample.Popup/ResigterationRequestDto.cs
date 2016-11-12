using PaymentSample.Common;

namespace PaymentSample.Popup
{
    public class ResigterationRequestDto : RequestDto
    {
        public string ProductItemCode { get; set; }
        public string PhoneNumber { get; set; }
    }
}