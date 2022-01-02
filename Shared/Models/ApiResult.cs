namespace Shared.Models
{
    public class ApiResult<T>
    {
        public int ResponseCode { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;

        public T Data { get; set; }
    }
}