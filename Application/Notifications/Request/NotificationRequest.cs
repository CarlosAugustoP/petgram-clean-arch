namespace Application.Notifications.Request
{
    public record NotificationRequest(Guid To, string Title, string Message);
}