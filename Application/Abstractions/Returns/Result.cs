using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.Returns
{
    public class Result<T>
    {
        public bool IsSuccess { get; private set; }
        public T Value { get; private set; }

        public Result(bool isSuccess, T value, string error)
        {
            IsSuccess = isSuccess;
            Value = value;
        }
        public static Result<T> Success(T value) => new Result<T>(true, value, null);
    }
}
