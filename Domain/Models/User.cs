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
        public List<User>? Followers { get; set; } = new List<User>();
        public List<User>? Following { get; set; } = new List<User>();
        public List<Post>? Posts { get; set; } = new List<Post>();
        public List<Moment>? Moments { get; set; } = new List<Moment>();
        public List<Pet>? Pets { get; set; } = new List<Pet>();
        public User()
        {
        }
    }
}
