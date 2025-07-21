using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Abstractions.DTOs.User
{
    public class MiniUserDto(string p, string n)
    {
        public string ProfilePicUrl = p;
        public string Name = n;
    }
}