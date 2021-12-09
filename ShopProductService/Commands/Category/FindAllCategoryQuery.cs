using MediatR;
using Shared.DTOs;
using System.Collections.Generic;

namespace ShopProductService.Commands.Category
{
    public class FindAllCategoryQuery : IRequest<List<CategoryDTO>>
    {
    }
}
