namespace API.Abstractions.Helpers
{
    public class ValidationErrorResponse
{
    public string Type { get; set; }
    public string Title { get; set; }
    public int Status { get; set; }
    public Dictionary<string, List<string>> Errors { get; set; } = new();
}
}