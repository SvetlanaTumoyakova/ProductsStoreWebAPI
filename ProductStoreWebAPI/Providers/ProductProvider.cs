using ProductsStore.DAL;
using ProductsStore.Models.Products;

namespace ProductsStore.WebAPI.Providers
{
    public class ProductProvider
    {
        private readonly DataBaseContext _dataBaseContext;
        public ProductProvider(DataBaseContext dataBaseContext)
        {
            _dataBaseContext = dataBaseContext;
        }

        //public async Task<List<Product>> GetProducts()
        //{
        //    var products = await _dataBaseContext.Products.Include(p => p.publisherHouse).ToListAsync();
        //    return products;
        //}

        //public async Task<List<Product>> Search(string searchString)
        //{
        //    List<Product> products;
        //    if (searchString != null)
        //    {
        //        products = await _dataBaseContext.Products
        //             .Where(b => b.BookTitle.ToLower().Contains(searchString.ToLower()) || b.Author.ToLower().Contains(searchString.ToLower()))
        //             .Include(p => p.publisherHouse)
        //             .ToListAsync();
        //    }
        //    else
        //    {
        //        products = await _dataBaseContext.Products.Include(p => p.publisherHouse).ToListAsync();
        //    }
        //    return products;
        //}
    }
}
