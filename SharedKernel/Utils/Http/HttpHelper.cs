using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace SharedKernel.Utils.Http
{
    public static class HttpHelper
    {
        public static T PostAsync<T>(string url, object? body = null)
        {
            using var client = new HttpClient();
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = client.PostAsync(url, content).Result;
            if (!response.IsSuccessStatusCode || response.Content == null)
            {
                throw new Exception(response.ReasonPhrase);
            }
            var responseString = response.Content.ReadAsStringAsync().Result;
            return responseString != null ? JsonSerializer.Deserialize<T>(responseString) : default!; 
        }

        public static T PostMultipartAsync<T>(string url, Dictionary<string, string> formData, Dictionary<string, byte[]> files, List<IFormFile>? images = null)
        {
            using var client = new HttpClient();
            using var content = new MultipartFormDataContent();

            foreach (var keyValuePair in formData)
            {
            content.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
            }

            foreach (var file in files)
            {
            var fileContent = new ByteArrayContent(file.Value);
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            content.Add(fileContent, file.Key, file.Key);
            }

            if (images != null)
            {
            foreach (var image in images)
            {
                var imageContent = new StreamContent(image.OpenReadStream());
                imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(image.ContentType);
                content.Add(imageContent, image.Name, image.FileName);
            }
            }

            var response = client.PostAsync(url, content).Result;
            if (!response.IsSuccessStatusCode || response.Content == null)
            {
            throw new Exception(response.ReasonPhrase);
            }
            var responseString = response.Content.ReadAsStringAsync().Result;
            return responseString != null ? JsonSerializer.Deserialize<T>(responseString)! : default!;
        }
    }
}