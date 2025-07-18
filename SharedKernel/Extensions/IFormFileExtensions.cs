using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace SharedKernel.Extensions
{
    public static class IFormFileExtensions
    {
        public static async Task<string> ToBase64StringAsync(this IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File cannot be null or empty", nameof(file));
            }

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            return Convert.ToBase64String(memoryStream.ToArray());
        }

        public static IFormFile FromBase64StringAsync(string base64String, string fileName, string contentType)
        {
            if (string.IsNullOrEmpty(base64String))
            {
                throw new ArgumentException("Base64 string cannot be null or empty", nameof(base64String));
            }

            var fileBytes = Convert.FromBase64String(base64String);
            var memoryStream = new MemoryStream(fileBytes);
            return new FormFile(memoryStream, 0, fileBytes.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };
        }

        public static Stream ToStream(this IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File cannot be null or empty", nameof(file));
            }

            var memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            memoryStream.Position = 0; 
            return memoryStream;
        }

        
    }
}