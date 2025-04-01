using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using SkiaSharp;

namespace SharedKernel.Utils.Files
{
    public static class FileConverter
    {
        public static byte[] StreamToByteArray(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        public static IFormFile Base64ToIFormFileImage(string base64String, string imgType)
        {
            var bytes = Convert.FromBase64String(base64String);
            var stream = new MemoryStream(bytes);

            using var skCodec = SKCodec.Create(stream)
            ?? throw new InvalidOperationException("Invalid media file");

            var (contentType, fileExtension) = skCodec.EncodedFormat switch
            {
                SKEncodedImageFormat.Jpeg => ("image/jpeg", "jpg"),
                SKEncodedImageFormat.Png => ("image/png", "png"),
                SKEncodedImageFormat.Gif => ("image/gif", "gif"),
                _ => throw new NotSupportedException("Unsupported image format")
            };

            stream.Position = 0;

            return new FormFile(stream, 0, stream.Length, "file", $"file.{fileExtension}")
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };

        }


    }
}
