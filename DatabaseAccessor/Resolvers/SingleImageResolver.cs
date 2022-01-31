using AutoMapper;
using DatabaseAccessor.Models;
using Shared.DTOs;

namespace DatabaseAccessor.Resolvers
{
	public class SingleImageResolver : IValueResolver<CartDetail, CartItemDTO, string>,
		IValueResolver<InvoiceDetail, OrderUserHistoryDTO, string>
	{
		public string Resolve(CartDetail source, CartItemDTO destination,
			string destMember, ResolutionContext context)
		{
			if (source.Product.Images == null)
				return string.Empty;
			return source.Product.Images.Split(";")[0];
		}

        public string Resolve(InvoiceDetail source, OrderUserHistoryDTO destination, string destMember, ResolutionContext context)
        {
			if (source.Product.Images == null)
				return string.Empty;
			return source.Product.Images.Split(";")[0];
		}
    }
}
