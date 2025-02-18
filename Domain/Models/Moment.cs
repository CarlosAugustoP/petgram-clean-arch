namespace Domain.Models
{
    public class Moment
    {
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public User Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Like>? Likes { get; set; }
        public List<User>? Viewers { get; set; }
        public Media media { get; set; }
        public Moment()
        {
        }
    }
}