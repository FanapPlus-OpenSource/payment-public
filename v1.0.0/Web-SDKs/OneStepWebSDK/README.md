# PIQO One Step Payment
You can use one-step payment, when you have user's phone number.
To use *PIQO* one-step payment, first you need to acquire a `GUID` from our system. Then, you must pass the `GUID` as an option to `pay()` function in javascript.
Here is an example on how to get the `GUID` from *PIQO*:

```C#
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
					
					var guid=transactionResponse.RequestId.ToString("N");

```


Now you may pass the guid to `TwoStepsPayment.htm`:

```javascript
function getParameterByName(name, url) {
    if (!url) {
      url = window.location.href;
    }
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}
function test(){
	var options={};
	options.request=getParameterByName('guid');
	options.appId=getParameterByName('appId');

	appson.payment.payDirect(options, 
										function(e) {
											if(e.IsSuccess)
												alert(e.TransactionCode);
											else
												alert(e.Error.Message.toString());
										 }
									   );
}

```