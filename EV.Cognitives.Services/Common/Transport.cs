using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FRDX.Cognitives.Services.Common
{
    public static class HttpResponseMessageExtension
    {

        public async static Task<T> DeserializeAsync<T>(this HttpResponseMessage response)
        {

            string JsonContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonConvert.DeserializeObject<T>(JsonContent);
            // 
        }
    }

 

 
        public class Transport 
    {

        private static HttpClient _httpClient { get; set; }
        public Transport(string serviceBaseUri, string apiKey)
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiKey);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.BaseAddress = new Uri(serviceBaseUri);
        }
        private HttpContent CreateHttpContent(object content)
        {
            var jsonData = JsonConvert.SerializeObject(content);
            return new StringContent(jsonData, Encoding.UTF8, "application/json");

        }
        public async Task<T> PostAsync<T>(string uri, object payload, CancellationToken token)
        {
            var content = CreateHttpContent(payload);
            T t = default(T);
            using (HttpResponseMessage Response = await _httpClient.PostAsync(uri, content, token))
            {
                if (Response.StatusCode == HttpStatusCode.OK ||
                    Response.StatusCode == HttpStatusCode.Created ||
                    Response.StatusCode == HttpStatusCode.Accepted ||
                    Response.StatusCode == HttpStatusCode.NoContent)
                {
                  
                        t = await Response.DeserializeAsync<T>();                  
                }
                else
                {
                    await ChechAndThrowAsync(Response);
                }
            }
            return t;
        }
        
        private async Task ChechAndThrowAsync(HttpResponseMessage response)
        {
            //TODO:Clean this code
            StringBuilder sbError = new StringBuilder();
            sbError.Append(response.ReasonPhrase);
            if (response.Content != null)
            {
                sbError.AppendLine();
                sbError.Append(await response.Content.ReadAsStringAsync());
            }

            throw new HttpRequestException(sbError.ToString());

        }
    }
   
}
