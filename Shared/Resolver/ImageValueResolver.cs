using AutoMapper;
using DatabaseAccessor.Model;
using Shared.DTOs;
using System;

namespace Shared.Resolver
{
    public class ImageValueResolver : IValueResolver<ShopProduct, ProductDTO, string[]>
    {
        public string[] Resolve(ShopProduct source, ProductDTO destination, string[] destMember, ResolutionContext context)
        {
            if (source.Images == null)
                return Array.Empty<string>();
            return source.Images.Split(";");
        }
    }
}
