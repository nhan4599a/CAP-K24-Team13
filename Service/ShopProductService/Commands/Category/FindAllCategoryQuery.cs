﻿using MediatR;
using Shared.DTOs;
using Shared.Models;

namespace ShopProductService.Commands.Category
{
    public class FindAllCategoryQuery : IRequest<PaginatedList<CategoryDTO>>
    {
        public PaginationInfo PaginationInfo { get; set; }
    }
}
