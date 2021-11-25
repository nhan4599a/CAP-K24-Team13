using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace DatabaseAccessor.Model
{
    public class ShopCategory
    {

        public int Id { get; set; }
      
        public int ShopId { get; set; }

        public string CategoryName { get; set; }

        public int Special { get; set; }

        public virtual List<ShopProduct> ShopProducts { get; set; }
        public string CatergoryName { get; set; }

        private ApplicationDbContext _dbContext;

        public ShopCategory(ApplicationDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }
        public IEnumerable<ShopCategory> displaydata { get; set; }
        public async Task onGet()
        {
            displaydata = await _dbContext.ToListAsync();
            ViewBag.displaydata = displaydata;
        }

        //@*@


    }
}
