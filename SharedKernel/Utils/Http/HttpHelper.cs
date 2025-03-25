using System.Text.Json;

namespace SharedKernel.Utils.Http
{
    public static class HttpHelper
    {
        public static async Task<Tresponse> HttpPostAsync<Tresponse>(string url, Dictionary<string, string>? headers, MultipartFormDataContent content)
        {
            using var client = new HttpClient();
            if (headers != null)
                foreach (var header in headers)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }

            var response = await client.PostAsync(url, content);
            if (!response.IsSuccessStatusCode || response.Content == null)
            {
                throw new Exception(response.ReasonPhrase);
            }

            var responseString = await response.Content.ReadAsStringAsync();
            return responseString != null ? JsonSerializer.Deserialize<Tresponse>(responseString)! : default!;
        }

    }
}