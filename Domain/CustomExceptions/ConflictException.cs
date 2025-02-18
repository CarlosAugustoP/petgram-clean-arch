using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CustomExceptions
{
    public class ConflictException : ApiException
    {
        public ConflictException(string message, string errorCode = "NOT_FOUND", string errorDetails = "")
           : base(message, 409, errorCode, errorDetails)
        {
        }
    }
}
