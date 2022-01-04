using AutoMapper;
using DatabaseAccessor.Models;
using DatabaseAccessor.Resolvers;
using Shared.DTOs;
using Shared.Models;

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
                cfg.CreateMap<CartItem, CartItemDto>();
            });
            _mapper = config.CreateMapper();
        }

        public static Mapper GetInstance()
        {
            return _instance ?? new Mapper();
        }

        public ProductDTO MapToProductDTO(ShopProduct product) => _mapper.Map<ProductDTO>(product);

        public CategoryDTO MapToCategoryDTO(ShopCategory category) => _mapper.Map<CategoryDTO>(category);

        public ShopInterfaceDTO MapToShopInterfaceDTO(ShopInterface shopInterface) => _mapper.Map<ShopInterfaceDTO>(shopInterface);
        public CartItemDto MapToCartItemDto(CartItem cartItem) => _mapper.Map<CartItemDto>(cartItem);
    }
}
