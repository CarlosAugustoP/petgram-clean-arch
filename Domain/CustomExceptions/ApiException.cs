using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CustomExceptions
{
    public class ApiException : Exception
    {
        public int StatusCode { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorDetails { get; set; }

        public ApiException(string message, int statusCode = 500, string errorCode = "UNKNOWN_ERROR", string errorDetails = "")
            : base(message)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
            ErrorMessage = message;
            ErrorDetails = errorDetails;
        }
    }

}
