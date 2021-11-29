using AutoMapper;
using DatabaseAccessor.Model;
using Shared.DTOs;

namespace Shared.Resolver
{
    public class ProductImageValueResolver : IValueResolver<ShopProduct, ProductDTO, string[]>
    {
        public string[] Resolve(ShopProduct source, ProductDTO destination, string[] destMember, ResolutionContext context)
        {
            return source.Images.Split(";");
        }
    }
}
