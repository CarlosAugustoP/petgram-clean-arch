using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Abstractions.Requests
{
    public record MessageRequest(string Content, Guid SentToId);
}