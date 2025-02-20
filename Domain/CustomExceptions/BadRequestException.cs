using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CustomExceptions
{
    public class BadRequestException : ApiException
    {
        public BadRequestException(string message, string errorCode = "BAD_REQUEST", string errorDetails = "")
            : base(message, 400, errorCode, errorDetails)
        {
        }
    }
}
