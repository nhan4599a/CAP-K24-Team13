using AutoMapper;
using DatabaseAccessor.Models;
using DatabaseAccessor.Resolvers;
using Shared.DTOs;

namespace DatabaseAccessor.Mapping
{
    public class Mapper
    {
        private static readonly Mapper _instance = null;

        private readonly IMapper _mapper;

        private Mapper()
        {
            MapperConfiguration config = new(cfg =>
            {
                cfg.CreateMap<ShopProduct, ProductDTO>()
                    .ForMember(target => target.CategoryName,
                        options => options.MapFrom(source => source.Category == null ? "" : source.Category.CategoryName))
                    .ForMember(target => target.Images,
                        options => options.MapFrom<ImageValueResolver>());

                cfg.CreateMap<ShopCategory, CategoryDTO>();

                cfg.CreateMap<ShopInterface, ShopInterfaceDTO>()
                    .ForMember(target => target.Images,
                        options => options.MapFrom<ImageValueResolver>());
            cfg.CreateMap<CartDetail, CartItemDTO>()
                .ForMember(target => target.ProductName,
                    options => options.MapFrom(source => source.Product.ProductName))
                .ForMember(target => target.Price,
                    options => options.MapFrom(source => source.Product.Price))
                .ForMember(target => target.Discount,
                    options => options.MapFrom(source => source.Product.Discount))
                .ForMember(target => target.Image,
                    options => options.MapFrom<SingleImageResolver>());
            });
            _mapper = config.CreateMapper();
        }

        public static Mapper GetInstance()
        {
            return _instance ?? new Mapper();
        }

        public ProductDTO MapToProductDTO(ShopProduct product) => _mapper.Map<ProductDTO>(product);

        public CategoryDTO MapToCategoryDTO(ShopCategory category) => _mapper.Map<CategoryDTO>(category);

        public ShopInterfaceDTO MapToShopInterfaceDTO(ShopInterface shopInterface) 
            => _mapper.Map<ShopInterfaceDTO>(shopInterface);

        public CartItemDTO MapToCartItemDTO(CartDetail cartItem) => _mapper.Map<CartItemDTO>(cartItem);

        public OrderUserHistoryDTO MapToOrderUserHistoryDTO(InvoiceDetail invoice) => _mapper.Map<OrderUserHistoryDTO>(invoice);
    }
}
