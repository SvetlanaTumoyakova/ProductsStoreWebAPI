using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsStore.DAL;
using ProductsStore.Models.Products;

namespace ProductsStore.WebAPI.Providers
{
    public class ProductProvider
    {
        private readonly DataBaseContext _dataBaseContext;
        private readonly IWebHostEnvironment _env;
        public ProductProvider(DataBaseContext dataBaseContext, IWebHostEnvironment env)
        {
            _dataBaseContext = dataBaseContext;
            _env = env;
        }

        public async Task<List<Product>> GetProducts()
        {
            var products = await _dataBaseContext.Products
                .Include(p => p.Category)
                .Include(p => p.ProductAttributes)
                .ToListAsync();

            return products;
        }
        public async Task<Product> GetProductById(Guid id)
        {
            var product = await _dataBaseContext.Products
                .Include(p => p.Category)
                .Include(p => p.ProductAttributes)
                .FirstOrDefaultAsync(p => p.Id == id);

            return product;
        }

        public async Task<List<Product>> SearchProductsByNameAsync(string? search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return await GetProducts();
            }

            //ILike: поиск без учета регистра
            var pattern = $"%{search}%";
            var products = await _dataBaseContext.Products
                .Where(p => EF.Functions.ILike(p.Name, pattern))
                .Include(p => p.Category)
                .Include(p => p.ProductAttributes)
                .ToListAsync();

            return products;
        }

    }
}
