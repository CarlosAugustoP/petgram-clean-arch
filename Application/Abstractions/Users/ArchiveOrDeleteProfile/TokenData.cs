using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models.UserAggregate;

namespace Application.Abstractions.Users.ArchiveOrDeleteProfile
{
    public class TokenData
    {
        public UserStatus Status { get; set; }
        public string Token { get; set; }

        public TokenData(UserStatus status, string token)
        {
            Status = status;
            Token = token;
        }
    }
}