using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public string Email { get; set; }
        public string? ProfileImgUrl { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string? Bio { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<User>? Followers { get; set; }
        public List<User>? Following { get; set; }
        public List<Post>? Posts { get; set; }
        public List<Moment>? Moments { get; set; }
        public List<Pet>? Pets { get; set; }
        public User()
        {
        }
    }
}
