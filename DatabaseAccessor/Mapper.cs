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
                cfg.CreateMap<ShopProduct, ProductWithCommentsDTO>()
                    .IncludeBase<ShopProduct, ProductDTO>();
                cfg.CreateMap<ShopProduct, MinimalProductDTO>();
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
                cfg.CreateMap<InvoiceDetail, OrderUserHistoryDTO>()
                    .ForMember(target => target.ProductName,
                        option => option.MapFrom(source => source.Product.ProductName))
                    .ForMember(target => target.Images,
                        option => option.MapFrom<SingleImageResolver>())
                    .ForMember(target => target.Created,
                        option => option.MapFrom(source => source.Invoice.Created));
                cfg.CreateMap<ProductComment, RatingDTO>()
                    .ForMember(target => target.ProductName,
                        option => option.MapFrom(source => source.Product.ProductName))
                    .ForMember(target => target.UserName,
                        option => option.MapFrom(source => source.User.UserName));

            });
            _mapper = config.CreateMapper();
        }

        public static Mapper GetInstance()
        {
            return _instance ?? new Mapper();
        }

        public ProductDTO MapToProductDTO(ShopProduct product) => _mapper.Map<ProductDTO>(product);

        public MinimalProductDTO MapToMinimalProductDTO(ShopProduct product) => _mapper.Map<MinimalProductDTO>(product);

        public ProductWithCommentsDTO MapToProductWithCommentsDTO(ShopProduct product) => _mapper.Map<ProductWithCommentsDTO>(product);

        public CategoryDTO MapToCategoryDTO(ShopCategory category) => _mapper.Map<CategoryDTO>(category);

        public ShopInterfaceDTO MapToShopInterfaceDTO(ShopInterface shopInterface)
            => _mapper.Map<ShopInterfaceDTO>(shopInterface);

        public CartItemDTO MapToCartItemDTO(CartDetail cartItem) => _mapper.Map<CartItemDTO>(cartItem);

        public OrderUserHistoryDTO MapToOrderUserHistoryDTO(InvoiceDetail invoice) => _mapper.Map<OrderUserHistoryDTO>(invoice);

        public RatingDTO MapToRatingDTO(ProductComment rating) => _mapper.Map<RatingDTO>(rating);
    }
}
