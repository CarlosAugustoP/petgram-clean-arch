namespace API.Abstractions.Result
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T Value { get; }
        private Result(bool isSuccess, T value)
        {
            IsSuccess = isSuccess;
            Value = value;
        }

        public static Result<T> Success(T value) => new Result<T>(true, value);
        public static Result<T> Failure(string error) => new Result<T>(false, default);

    }
}