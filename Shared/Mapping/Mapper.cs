using AutoMapper;
using DatabaseAccessor.Model;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Mapping
{
    public class Mapper
    {

        private static Mapper _instance = null;

        private IMapper _mapper;

        private Mapper()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ShopProduct, ProductDTO>()
                    .ForMember(target => target.CategoryName,
                        options => options.MapFrom(source => source.Category == null ? "" : source.Category.CategoryName))
                    .ForMember(target => target.Images,
                        options => options.MapFrom(source => source.ImageSet == null ? null :
                            new string[] { source.ImageSet.Image1, source.ImageSet.Image2, source.ImageSet.Image3, source.ImageSet.Image4, source.ImageSet.Image5 }));

                cfg.CreateMap<ShopProduct, CategoryDTO>();
            });
            _mapper = config.CreateMapper();
        }

        public static Mapper GetInstance()
        {
            return _instance ?? new Mapper();
        }

        public ProductDTO MapToProductDTO(ShopProduct product) => _mapper.Map<ProductDTO>(product);
        public CategoryDTO MapToCategoryDTO(ShopCategory category) => _mapper.Map<CategoryDTO>(category);
    }
}
