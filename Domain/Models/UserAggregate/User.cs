namespace Domain.Models.UserAggregate
{
    public class User
    {
        public Guid Id { get; set; }
        public UserStatus Status { get; private set; }
        public string Email { get; set; }
        public string ProfileImgUrl { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string? Bio { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<User>? Followers { get; set; } = new List<User>();
        public List<User>? Following { get; set; } = new List<User>();
        public List<Post>? Posts { get; set; } = new List<Post>();
        public List<Moment>? Moments { get; set; } = new List<Moment>();
        public List<Pet>? Pets { get; set; } = new List<Pet>();
        public UserRole Role { get; set; } = UserRole.COMMON;
        public DateTime? LastLogin { get; private set; }
        public Preference Preference { get; set; }
        public User()
        {
        }

        public void BanUser()
        {
            Status = UserStatus.BANNED;
        }
        public void ActivateUser()
        {
            Status = UserStatus.ACTIVE;
        }
        public void InactivateUser()
        {
            Status = UserStatus.INACTIVE;
        }
        public void ArchiveUser()
        {
            Status = UserStatus.ARCHIVED;
        }
        public bool IsBanned()
        {
            return Status == UserStatus.BANNED;
        }
        public void SetLastLogin(DateTime lastLogin)
        {
            LastLogin = lastLogin;
        }

    }
}
