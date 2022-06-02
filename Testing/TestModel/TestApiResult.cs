using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestModel
{
    public class TestApiResult
    {
        [Fact]
        public void Test()
        {
            var model = ApiResult.SucceedResult;
            Assert.Equal(200, model.ResponseCode);
            Assert.Empty(model.ErrorMessage);
        }
    }
}
