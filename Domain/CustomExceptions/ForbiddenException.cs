using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.CustomExceptions
{
    public class ForbiddenException : ApiException
    {
        public ForbiddenException(string message, string errorCode = "FORBIDDEN", string errorDetails = "")
            : base(message, 403, errorCode, errorDetails)
        {
        }
    }
}