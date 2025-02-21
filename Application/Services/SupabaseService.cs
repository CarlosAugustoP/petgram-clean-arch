using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supabase.Storage;
using Supabase;
using Supabase.Interfaces;
using Domain.CustomExceptions;
using SharedKernel.Utils.Files;

namespace Application.Services
{
    public interface ISupabaseService
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName, string bucketName);
    }
    public class SupabaseService : ISupabaseService
    {
        private readonly string _supabaseKey;
        private readonly string _supabaseUrl;
        private readonly Supabase.Client _supabaseClient;

        public SupabaseService(string supabaseKey, string supabaseUrl)
        {
            _supabaseKey = supabaseKey;
            _supabaseUrl = supabaseUrl;
            _supabaseClient = new Supabase.Client(supabaseUrl, supabaseKey);
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string bucketName)
        {
            
            var storage = _supabaseClient.Storage.From(bucketName);
            var fileToBytes = FileConverter.StreamToByteArray(fileStream);
            var response = await storage.Upload(fileToBytes, fileName);
            if (response != null)
            {
                return $"{storage.GetPublicUrl}/object/public/{bucketName}/{fileName}";
            }else
            {
                throw new BadRequestException("Could not process the image");
            }
        }
    }
}
