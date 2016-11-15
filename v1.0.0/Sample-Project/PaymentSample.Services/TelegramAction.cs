using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using PaymentSample.Common;
using PaymentSample.Common.Actions;
using ServiceStack.Text;

namespace PaymentSample.Services
{
    [Export("telegram", typeof(IAction))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class TelegramAction : InformationAction
    {
        private const string ServiceUrl = "/api/telegram/charge";


        public override List<string> HelpInformation => new List<string>();

        public override List<string> Act(string command)
        {
            var strings = command.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);
            if (strings.Length < 2)
                return new List<string> {"Must enter telegram Id"};

            var jsonSerializer = new JsonSerializer<ChargeRequestDto>();
            var dto = new ChargeRequestDto
            {
                Date = DateTime.UtcNow.ToString("yyyyMMddHHmmss"),
                ProductCode = ProductCode,
                ProductItemCode = ProductItemCode,
                ReferenceId = Guid.NewGuid().ToString("N"),
                RUID = Guid.NewGuid().ToString("N"),
                Token = strings[1]
            };

            var text = jsonSerializer.SerializeToString(dto);
            var sign = SignatureHelper.Sign(SignKey, text);

            var transactionResponse =
                ServiceUtility.Post<TransactionResponse>(BaseAddress + ServiceUrl,
                    text,
                    new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("TELEGRAM", dto.Token),
                        new KeyValuePair<string, string>("RUID", dto.RUID),
                        new KeyValuePair<string, string>("PRODUCT", dto.ProductCode),
                        new KeyValuePair<string, string>("REQUESTDATE", dto.Date),
                        new KeyValuePair<string, string>("SIGNATURE", sign)
                    });

            return new List<string>
            {
                (transactionResponse?.IsSuccess ?? false)
                    ? transactionResponse.TransactionCode
                    : (transactionResponse?.Error.ToString() ?? "ERROR")
            };
        }
    }
}