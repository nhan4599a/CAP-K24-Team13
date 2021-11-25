using DatabaseAccessor.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseAccessor
{
    internal class ApiResult<T> : Task<IEnumerable<ShopCategory>>
    {
        public int ResponseCode { get; set; }
        public object Data { get; set; }
    }
}