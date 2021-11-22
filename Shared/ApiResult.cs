using System;

namespace Shared
{
    public class ApiResult<T>
    {
        public string ErrorMessage { get; set; }

        public T Data { get; set; }

        public int ResponseCode { get; set; }
    }
}
