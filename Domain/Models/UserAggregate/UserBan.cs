using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models.UserAggregate;

namespace Domain.Models.UserAggregate
{
    public class UserBan
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public BanReason Reason { get; set; }
        public DateTime BannedAt { get; set; }
        public string? Remark { get; set; }
        public DateTime ToBeUnbannedAt { get; set; }

        public UserBan() { }

        public UserBan(Guid id, Guid userId, User? user, BanReason reason, DateTime bannedAt, DateTime unbannedAt, string? remark = null)
        {
            Id = id;
            UserId = userId;
            User = user;
            Reason = reason;
            BannedAt = bannedAt;
            ToBeUnbannedAt = unbannedAt;
            Remark = remark;
        }
    }
}