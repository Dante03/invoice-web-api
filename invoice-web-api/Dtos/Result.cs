using invoice_web_api.Enums;

namespace invoice_web_api.Dtos
{
    public class Result<T>
    {
        public bool Success { get; private set; }
        public string Code { get; set; }
        public ErrorType? ErrorType { get; set; }
        public string Error { get; private set; }
        //public T Data { get; private set; }

        public T Data { get; private set; }
        public IEnumerable<T> DataList { get; private set; }

        public static Result<T> Ok(T data)
            => new Result<T> { Success = true, Data = data };

        public static Result<T> Ok(IEnumerable<T> dataList)
        => new Result<T>
        {
            Success = true,
            DataList = dataList
        };
        public static Result<T> Fail(string code, string error, ErrorType errorType, T? data = default)
        => new Result<T>
        {
            Code = code,
            Success = false,
            Error = error,
            ErrorType = errorType,
            Data = data
        };

        public static Result<T> NotFound(string code, string error, ErrorType errorType)
        => new Result<T>
        {
            Code = code,
            Success = true,
            Error = error,
            ErrorType = errorType
        };
    }
}
