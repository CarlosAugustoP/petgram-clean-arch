namespace Domain.Models.NotificationAggregate
{
    public enum NotificationTrigger
    {
        LIKED_POST,
        COMMENTED_POST,
        NEW_FOLLOWER,
        NEW_MESSAGE,
        POST_FINISHED_UPLOAD
    }
}