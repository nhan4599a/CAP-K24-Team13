using DatabaseAccessor.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseAccessor
{
    internal class ApiResult<T>
    {
        public int ResponseCode { get; set; }
        public object Data { get; set; }
    }
}