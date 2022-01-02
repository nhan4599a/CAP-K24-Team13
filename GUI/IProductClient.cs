using Refit;
using Shared;
using Shared.DTOs;
using Shared.RequestModels;
using System.Collections;
using System.Threading.Tasks;

namespace GUI
{
    public interface IProductClient
    {
        [Get("/products?requestModel.paginationInfo.pageNumber={pageNumber}&requestModel.paginationInfo.pageSize={pageSize}")]
        Task<PaginatedList<ProductDTO>> GetProductsAsync(int pageNumber, int? pageSize);
    }
}