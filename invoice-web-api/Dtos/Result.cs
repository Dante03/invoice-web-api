using invoice_web_api.Enums;

namespace invoice_web_api.Dtos
{
    public class Result<T>
    {
        public bool Success { get; private set; }
        public ErrorType? ErrorType { get; set; }
        public string Error { get; private set; }
        //public T Data { get; private set; }

        public IEnumerable<T> Data { get; private set; }

        public static Result<T> Ok(T data)
            => new Result<T> { Success = true, Data = new[] { data } };

        public static Result<T> Ok(IEnumerable<T> data)
        => new Result<T>
        {
            Success = true,
            Data = data
        };
        public static Result<T> Fail(string error, ErrorType errorType)
        => new Result<T>
        {
            Success = false,
            Error = error,
            ErrorType = errorType
        };
    }
}
