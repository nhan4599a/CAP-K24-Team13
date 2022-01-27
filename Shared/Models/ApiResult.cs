namespace Shared.Models
{
    public class ApiResult<T> : ApiResult
    {
        public T Data { get; set; }

        public static ApiResult<T> CreateSuccessResult(T data)
        {
            return new ApiResult<T>
            {
                Data = data,
                ResponseCode = 200
            };
        }
    }

    public class ApiResult
    {
        public int ResponseCode { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;

        public static ApiResult SucceedResult => new()
        {
            ResponseCode = 200
        };

        public static ApiResult CreateErrorResult(int responseCode, string errorMessage)
        {
            return new ApiResult
            {
                ErrorMessage = errorMessage,
                ResponseCode = responseCode
            };
        }
    }
}