using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SharedKernel.Utils.Http
{
    public static class HttpHelper
    {
        public static async Task<Tresponse> HttpPostAsync<Tresponse>(string url, object? body = null)
        {
            using var client = new HttpClient();
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            if (!response.IsSuccessStatusCode || response.Content == null)
            {
                throw new Exception(response.ReasonPhrase);
            }
            var responseString = response.Content.ReadAsStringAsync().Result;
            return responseString != null ? JsonSerializer.Deserialize<Tresponse>(responseString)! : default!;
        }

        

    }
}