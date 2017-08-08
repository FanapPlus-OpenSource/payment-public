# How does Piqo work

Piqo provides multiple ways of payment over multiple mobile operators and banks. from there variety of methods, mobile operators/banks implement some. The following is a list of payment methods and a short description of them, where they are implemented and how they can be used.

## Direct charging (AKA Api Charging)

In this method Piqo charges a customer (using their AccountId or token) directly.

for charging a user using her token (acquired via authenticating with [Appson Identity Service][1]) one must issue a request like this:

````http
POST /api/token/charge
host: https://pg.appson.ir/
SIGNATURE: signed-request-body
PRODUCT: product-code-as-shown-in-panel
REQUESTDATE: request-date-with-yyyyMMddHHmmss-format
RUID: unique-request-identifier-in-GUID-format
---other headers are omitted for clarity---

{
     "Date": "<request-date>",
     "ProductCode": "<product-code-as-shown-in-panel>",
     "RUID": "<unique-request-identifier-in-GUID-format>",
     "ProductItemCode": "<product-item-code-as-shown-in-panel>",
     "ReferenceId": "<reference id>",
     "PaymentMethod": "<a number indicating payment method (listed below)>"
}
````

* 0 = charging through `MCI / IMI` payment gateway
* 1 = charging through `MTN-Irancell` payment gateway
* 2 = charging through `Pasargad Bank DirectDebit` payment gateway

The return type will be an object like bellow:

````csharp
{
    bool IsSuccess,
    string TransactionCode,
    AppsonErrorResponse Error;
    DateTime? SubscriptionExpireDate;
    bool? InsufficientOperatorBalance;
}
````

in which the `AppsonErrorResponse` is:

````csharp
{
    string Code;
    string Message;
    string Description;
    string[] Params;
}
````

representing an error should one occure.

[1]: https://github.com/appson/identity-public/