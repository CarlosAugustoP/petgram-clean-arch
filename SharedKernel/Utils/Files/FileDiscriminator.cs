using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SharedKernel.Utils.Files
{
    public static class FileDiscriminator
    {
        private static readonly string[] ImageMimeTypes = { "image/jpeg", "image/png", "image/gif", "image/bmp", "image/tiff" };
        private static readonly string[] VideoMimeTypes = { "video/mp4", "video/mpeg", "video/webm", "video/ogg", "video/quicktime" };

        public static bool IsImage(IFormFile file)
        {
            return ImageMimeTypes.Contains(file.ContentType);
        }
        public static bool IsVideo(IFormFile file)
        {
            return VideoMimeTypes.Contains(file.ContentType);
        }

    }
}
