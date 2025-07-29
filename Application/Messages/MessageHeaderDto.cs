using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Messages
{
    public class MessageHeaderDto
    {
        public string UserName { get; set; }
        public string UserProfileImageUrl { get; set; }
        public DateTime LastMessageDate { get; set; }
        public string LastMessage { get; set; }
        public Guid UserId { get; set; }
        public int UnreadMessagesCount { get; set; }
        
        public MessageHeaderDto(string userName, string userProfileImageUrl, DateTime lastMessageDate, string lastMessage, Guid userId, int unreadMessagesCount)
        {
            UserName = userName;
            UserProfileImageUrl = userProfileImageUrl;
            LastMessageDate = lastMessageDate;
            LastMessage = lastMessage;
            UserId = userId;
            UnreadMessagesCount = unreadMessagesCount;
        }
    }
}