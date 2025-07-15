using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UserManagement
{
    public class UserBanOptions
    {
        public BanTimeEnum BanDuration { get; set; }
        public string? Remark { get; set; }
    }
}