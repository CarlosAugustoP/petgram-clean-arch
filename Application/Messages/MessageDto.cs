using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace Application.Messages
{
    [AutoMap(typeof(Domain.Models.Message))]
    public class MessageDto
    {
        public required string Content { get; set; }
        public required string CreatedAt { get; set; }
        public required bool IsRead { get; set; }
        public required bool IsEdited { get; set; }
    }
}