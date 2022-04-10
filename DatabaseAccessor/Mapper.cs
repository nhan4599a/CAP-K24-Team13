using AutoMapper;
using DatabaseAccessor.Models;
using DatabaseAccessor.Resolvers;
using Shared.DTOs;
using Shared.Models;
using System;
using System.Linq;

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
                    .IncludeBase<ShopProduct, MinimalProductDTO>()
                    .ForMember(target => target.CategoryName,
                        options => options.MapFrom(source => source.Category == null ? "" : source.Category.CategoryName));
                cfg.CreateMap<ShopProduct, ProductWithCommentsDTO>()
                    .IncludeBase<ShopProduct, ProductDTO>();
                cfg.CreateMap<ShopProduct, MinimalProductDTO>()
                    .ForMember(target => target.Images,
                        options => options.MapFrom<ImageValueResolver>());
                cfg.CreateMap<ShopCategory, CategoryDTO>()
                    .ForMember(target => target.ProductCount, 
                        options => options.MapFrom(source => source.ShopProducts.Count));

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
                cfg.CreateMap<Invoice, OrderDTO>()
                    .ForMember(target => target.CustomerName,
                        options => options.MapFrom(source => source.FullName));
                cfg.CreateMap<InvoiceDetail, OrderItemDTO>()
                    .ForMember(target => target.ProductName,
                        option => option.MapFrom(source => source.Product.ProductName))
                    .ForMember(target => target.Image,
                        option => option.MapFrom<SingleImageResolver>())
                    .ForMember(target => target.CreatedAt,
                        option => option.MapFrom(source => source.Invoice.CreatedAt))
                    .ForMember(target => target.Status,
                        option => option.MapFrom(source => source.Invoice.Status));
                cfg.CreateMap<ProductComment, RatingDTO>()
                    .ForMember(target => target.ProductName,
                        option => option.MapFrom(source => source.Product.ProductName))
                    .ForMember(target => target.UserName,
                        option => option.MapFrom(source => source.User.UserName));
                cfg.CreateMap<Invoice, InvoiceDTO>()
                    .ForMember(target => target.TotalPrice,
                        options => options.MapFrom(source => source.Details.Sum(detail => detail.Price * detail.Quantity)))
                    .ForMember(target => target.PhoneNumber,
                        options => options.MapFrom(source => source.Phone))
                    .ForMember(target => target.IsReported,
                        options => options.MapFrom(source => source.Report != null));

                cfg.CreateMap<Report, ReportDTO>()
                    .ForMember(target => target.Reporter,
                        options => options.MapFrom(source => source.Reporter.UserName))
                    .ForMember(target => target.AffectedUser,
                        options => options.MapFrom(source => source.AffectedUser.UserName));

                cfg.CreateMap<Invoice, InvoiceDetailDTO>()
                    .IncludeBase<Invoice, InvoiceDTO>()
                    .ForMember(target => target.Products,
                        options => options.MapFrom(source => source.Details));

                cfg.CreateMap<User, UserDTO>()
                    .ForMember(target => target.BirthDay,
                        options => options.MapFrom(source => source.DoB))
                    .ForMember(target => target.IsConfirmed,
                        options => options.MapFrom(source => source.EmailConfirmed))
                    .ForMember(target => target.FullName,
                        options => options.MapFrom(source => $"{source.FirstName} {source.LastName}"))
                    .ForMember(target => target.IsLockedOut,
                        options => options.MapFrom(source => source.LockoutEnd > DateTimeOffset.Now
                            && source.Status == AccountStatus.Available))
                    .ForMember(target => target.IsAvailable,
                        options => options.MapFrom(source => source.Status == AccountStatus.Available))
                    .ForMember(target => target.Role,
                        options => options.MapFrom(source => source.UserRoles[0].Role.Name))
                    .ForMember(target => target.ReportCount,
                        options => options.MapFrom(source => source.AffectedReports.Count));
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

        public OrderItemDTO MapToOrderItemDTO(InvoiceDetail invoiceDetail) => _mapper.Map<OrderItemDTO>(invoiceDetail);

        public OrderDTO MapToOrderDTO(Invoice invoice) => _mapper.Map<OrderDTO>(invoice);

        public RatingDTO MapToRatingDTO(ProductComment rating) => _mapper.Map<RatingDTO>(rating);

        public InvoiceDTO MapToInvoiceDTO(Invoice invoice) => _mapper.Map<InvoiceDTO>(invoice);

        public ReportDTO MapToReportDTO(Report report) => _mapper.Map<ReportDTO>(report);

        public InvoiceDetailDTO MapToInvoiceDetailDTO(Invoice invoice) => _mapper.Map<InvoiceDetailDTO>(invoice);

        public UserDTO MapToUserDTO(User user) => _mapper.Map<UserDTO>(user);
    }
}
