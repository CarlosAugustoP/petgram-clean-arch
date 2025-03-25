using System.Diagnostics.CodeAnalysis;
using Application.Services;
using Domain.CustomExceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using SharedKernel.Utils.Http;

namespace Application.Abstractions.Pets.GetTypeQuery
{
    public class ExternalApiConfiguration{
        public required string ApiUrl {get; set;} 
        public required string ApiKey {get; set;}

        [SetsRequiredMembers]
        public ExternalApiConfiguration(string apiUrl, string apiKey){
            ApiUrl = apiUrl;
            ApiKey = apiKey;
        } 
    }

    public sealed record GetTypeQuery : IRequest<Dictionary<string, string>>
    {
        public required IFormFile File {get; set;}
        [SetsRequiredMembers]
        public GetTypeQuery(IFormFile file)
        {
            File = file;
        }
    }

    internal class ModelResponse
    {
        public required string Class { get; set; }
        public required decimal Confidence { get; set; }
        public required string Message { get; set; }

    }
    public sealed class GetTypeQueryHandler : IRequestHandler<GetTypeQuery, Dictionary<string, string>>
    {
        private readonly ISupabaseService _supabaseService;
        private readonly ExternalApiConfiguration _externalApiConfiguration;

        public GetTypeQueryHandler(ISupabaseService supabaseService, ExternalApiConfiguration externalApiConfiguration)
        {
            _externalApiConfiguration = externalApiConfiguration;
            _supabaseService = supabaseService;
        }

        public async Task<Dictionary<string, string>> Handle(GetTypeQuery request, CancellationToken cancellationToken)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Api-Key", _externalApiConfiguration.ApiKey);
            var content = new MultipartFormDataContent
            {
                { new StringContent(request.File.FileName), "file" }
            };
            object response;
            try
            {
                response = await HttpHelper.HttpPostAsync<ModelResponse>(_externalApiConfiguration.ApiUrl!, content);
            }
            catch (Exception e)
            {
                throw new ApiException("Failed to send the image to foreign API." + e.Message);
            }
            if (response is ModelResponse modelResponse)
            {
                var url = await _supabaseService.UploadFileAsync(request.File.OpenReadStream(),
                    $"{Guid.NewGuid()}_{request.File.FileName}", "petgram-pets");
                
                return new Dictionary<string,string>
                {
                    {"type", modelResponse.Class},
                    {"url", url}
                };
            }
            else 
            {
                throw new ApiException("Failed to send the request to foreign API");
            }
        }
    }
}