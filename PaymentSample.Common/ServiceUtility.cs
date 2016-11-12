using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using ServiceStack.Text;

namespace PaymentSample.Common
{
    public static class ServiceUtility
    {
        public static T Get<T>(string url)
        {
            using (var client = new HttpClient())
            {
                var clientTask = client.GetStringAsync(url).ContinueWith(m =>
                {
                    try
                    {
                        var serializer = new JsonSerializer<T>();
                        var response = serializer.DeserializeFromString(m.Result);
                        return response;
                    }
                    catch (Exception)
                    {
                        return default(T);
                    }
                });
                clientTask.Wait();
                return clientTask.Result;
            }
        }

        public static T Post<T>(string url, string request, List<KeyValuePair<string, string>> headers = null)
        {
            var serializer = new JsonSerializer<T>();

            using (var client = new HttpClient())
            {
                if ((headers != null) && headers.Any())
                    headers.ForEach(m => client.DefaultRequestHeaders.Add(m.Key, m.Value));
                var clientTask =
                    client.PostAsync(url, new StringContent(request, Encoding.UTF8, "application/json"))
                        .ContinueWith(m =>
                        {
                            var task = m.Result.Content.ReadAsStringAsync().ContinueWith(content =>
                            {
                                try
                                {
                                    var response = serializer.DeserializeFromString(content.Result);
                                    return response;
                                }
                                catch (Exception)
                                {
                                    return default(T);
                                }
                            });
                            task.Wait();
                            return task.Result;
                        });
                clientTask.Wait();
                return clientTask.Result;
            }
        }
    }
}