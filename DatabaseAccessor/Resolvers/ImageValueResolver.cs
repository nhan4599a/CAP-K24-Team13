using AutoMapper;
using DatabaseAccessor.Models;
using Shared.DTOs;
using System;

namespace DatabaseAccessor.Resolvers
{
    public class ImageValueResolver :
        IValueResolver<ShopProduct, ProductDTO, string[]>,
        IValueResolver<ShopInterface, ShopInterfaceDTO, string[]>
    {
        public string[] Resolve(ShopProduct source, ProductDTO destination, string[] destMember, ResolutionContext context)
        {
            if (source.Images == null)
                return Array.Empty<string>();
            return source.Images.Split(";");
        }

        public string[] Resolve(ShopInterface source, ShopInterfaceDTO destination, string[] destMember, ResolutionContext context)
        {
            if (source.Images == null)
                return Array.Empty<string>();
            return source.Images.Split(";");
        }
    }
}
