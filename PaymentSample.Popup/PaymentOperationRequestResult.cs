using System;

namespace PaymentSample.Popup
{
    public class PaymentOperationRequestResult
    {
        public string ErrorMessage { get; set; }
        public bool IsSuccess { get; set; }
        public Guid RequestId { get; set; }
    }
}