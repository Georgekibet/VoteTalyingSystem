using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Mobile.Common.Net
{
    public class BaseClient
    {
        public const string JsonContentType = "application/json";

        public async Task<T> GetJson<T>(string baseAddress, string endpoint)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = Timeouts.DefaultHttpTimeout();
                client.BaseAddress = new Uri(baseAddress);

                var response = await client.GetAsync(endpoint);
                var content = response.Content;

                var text = await content.ReadAsStringAsync();
                if(string.IsNullOrEmpty(text))
                     throw new NullReferenceException("Error Occured");
                return JsonConvert.DeserializeObject<T>(text);
            }              
        }

        public async Task<T> PostJson<R, T>(R request, string baseAddress, string endpoint, int? timeOutSeconds = null)
        {
            using (var client = new HttpClient())
            {
                var timeout = timeOutSeconds.HasValue ? new TimeSpan(0, 0, timeOutSeconds.Value) : Timeouts.DefaultHttpTimeout();
                client.Timeout = timeout;
                client.BaseAddress = new Uri(baseAddress);

                var response = await client.PostAsync(endpoint, CreateJsonContent(request));
                var content = response.Content;
                
                var text = await content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(text);
            }              
        }

        public StringContent CreateJsonContent(object request)
        {
            var stringRequest = request as string ?? JsonConvert.SerializeObject(request);
            return new StringContent(stringRequest, Encoding.UTF8, JsonContentType);
        }
    }
}
