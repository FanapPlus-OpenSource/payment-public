using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using PaymentSample.Common;
using PaymentSample.Common.Actions;
using ServiceStack.Text;

namespace PaymentSample.Popup
{
    [Export("phone", typeof(IAction))]
    public class PhoneAction : InformationAction
    {
       public override List<string> HelpInformation => new List<string>
        {
            "phone\tcreate a request for input phone number",
            "\tusage:",
            "\tphone [phone number]"
        };

        public override List<string> Act(string command)
        {
            var strings = command.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);
            if (strings.Length < 2)
                return new List<string> {"Must enter phone number"};
            var jsonSerializer = new JsonSerializer<ResigterationRequestDto>();

            var dto = new ResigterationRequestDto
            {
                Date = DateTime.UtcNow.ToString("yyyyMMddHHmmss"),
                ProductCode = ProductCode,
                ProductItemCode = ProductItemCode,
                RUID = Guid.NewGuid().ToString("N"),
                PhoneNumber = strings[1]
            };

            var text = jsonSerializer.SerializeToString(dto);
            var sign = SignatureHelper.Sign(SignKey, text);

            var transactionResponse =
                ServiceUtility.Post<PaymentOperationRequestResult>(
                    $"{BaseAddress}/api/web/PaymentOperationRequest/sendParametersWithPhoneNumber",
                    text,
                    new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("RUID", dto.RUID),
                        new KeyValuePair<string, string>("PRODUCT", dto.ProductCode),
                        new KeyValuePair<string, string>("REQUESTDATE", dto.Date),
                        new KeyValuePair<string, string>("SIGNATURE", sign)
                    });

            return new List<string>
            {
                (transactionResponse?.IsSuccess ?? false)
                    ? transactionResponse.RequestId.ToString("N")
                    : (transactionResponse?.ErrorMessage ?? "ERROR")
            };
        }
    }
}