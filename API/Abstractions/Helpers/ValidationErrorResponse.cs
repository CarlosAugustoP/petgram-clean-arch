namespace API.Abstractions.Helpers
{
    public class ValidationErrorResponse
    {
        public Dictionary<string, List<string>> Errors { get; set; } = new();
    }
}