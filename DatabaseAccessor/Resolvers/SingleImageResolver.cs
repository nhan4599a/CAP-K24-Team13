using AutoMapper;
using DatabaseAccessor.Models;
using Shared.DTOs;

namespace DatabaseAccessor.Resolvers
{
	public class SingleImageResolver : IValueResolver<CartDetail, CartItemDTO, string>,
		IValueResolver<InvoiceDetail, OrderDTO, string>
	{
		public string Resolve(CartDetail source, CartItemDTO destination,
			string destMember, ResolutionContext context)
		{
			if (source.Product.Images == null)
				return string.Empty;
			return source.Product.Images.Split(";")[0];
		}

        public string Resolve(InvoiceDetail source, OrderDTO destination, string destMember, ResolutionContext context)
        {
			if (source.Product.Images == null)
				return string.Empty;
			return source.Product.Images.Split(";")[0];
		}
    }
}
