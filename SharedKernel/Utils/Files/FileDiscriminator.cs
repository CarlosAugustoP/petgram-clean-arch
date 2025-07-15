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

        public static string DetermineMediaType(IFormFile media)
        {
            if (IsImage(media)) return "image";
            if (IsVideo(media)) return "video";
            //TODO add more file types
            else throw new ArgumentException("Unsupported file type");
        }

        public static string DetermineMediaType(byte[] bytes)
        {
            if (bytes.Length < 4)
                throw new ArgumentException("Arquivo muito pequeno para ser uma mídia válida.");

            if (bytes[0] == 0xFF && bytes[1] == 0xD8) return "image/jpeg"; 
            if (bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E && bytes[3] == 0x47) return "image/png";
            if (bytes[0] == 0x47 && bytes[1] == 0x49 && bytes[2] == 0x46) return "image/gif"; 

            if (bytes[0] == 0x00 && bytes[1] == 0x00 && bytes[2] == 0x00 && bytes[3] == 0x20 &&
                bytes[4] == 0x66 && bytes[5] == 0x74 && bytes[6] == 0x79 && bytes[7] == 0x70)
                return "video/mp4";

            if (bytes[0] == 0x52 && bytes[1] == 0x49 && bytes[2] == 0x46 && bytes[3] == 0x46 &&
                bytes[8] == 0x41 && bytes[9] == 0x56 && bytes[10] == 0x49)
                return "video/x-msvideo";
            if (bytes[0] == 0x1A && bytes[1] == 0x45 && bytes[2] == 0xDF && bytes[3] == 0xA3)
                return "video/x-matroska";

            if (bytes[4] == 0x66 && bytes[5] == 0x74 && bytes[6] == 0x79 && bytes[7] == 0x70 &&
                bytes[8] == 0x71 && bytes[9] == 0x74 && bytes[10] == 0x20 && bytes[11] == 0x20)
                return "video/quicktime";

            if (bytes[0] == 0x49 && bytes[1] == 0x44 && bytes[2] == 0x33)
                return "audio/mpeg";

            throw new NotSupportedException("Formato de mídia não suportado.");
        }

        public readonly static List<string> VideoTypes = new List<string> { "video/mp4", "video/mpeg", "video/webm", "video/ogg", "video/quicktime" };
        public readonly static List<string> ImageTypes = new List<string> { "image/jpeg", "image/png", "image/gif"};

    }
}
