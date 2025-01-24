using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Users
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string profileImgUrl { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Users> Followers { get; set; }
        public List<Users> Following { get; set; }
        public List<Post> Posts { get; set; }
        public List<Pet> Pets { get; set; }
        public Users()
        {
        }
    }
}
