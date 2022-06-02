using Shared.RequestModels;
using ShopInterfaceService.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestShopInterfaceService
{
    public class TestCreateOrEditShopInterfaceCommand
    {
        [Fact]
        public void Test()
        {
            var model = new CreateOrEditShopInterfaceCommand
            {
                ShopId = 1,
                RequestModel = new CreateOrEditInterfaceRequestModel
                {
                    Avatar = "abc.jpg",
                    ShopImages = new string[] { "xyz.jpg", "ncc.jpg" }
                }
            };
            Assert.Equal(1, model.ShopId);
            Assert.NotNull(model.RequestModel);
        }
    }
}
