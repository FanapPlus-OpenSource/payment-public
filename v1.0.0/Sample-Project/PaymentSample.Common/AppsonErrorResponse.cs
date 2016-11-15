    using ServiceStack.Text;

namespace PaymentSample.Common
{
    public class AppsonErrorResponse
    {
        public AppsonErrorResponse()
        {
        }

        public AppsonErrorResponse(string message, string[] @params)
        {
            Message = message;
            Params = @params;
        }

        public string Message { get; set; }
        public string[] Params { get; set; }

        public override string ToString()
        {
            var serializer = new JsonSerializer<AppsonErrorResponse>();
            var serializedResponse = serializer.SerializeToString(this);
            return serializedResponse;
        }
    }
}