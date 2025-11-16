using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsStore.DAL;
using ProductsStore.Models.Products;
using ProductsStore.WebAPI.DTO.Product;
using System.Linq;

namespace ProductsStore.WebAPI.Providers
{
    public class ProductProvider
    {
        private readonly DataBaseContext _dataBaseContext;
        private readonly IWebHostEnvironment _env;

        public const int count = 10;
        public ProductProvider(DataBaseContext dataBaseContext, IWebHostEnvironment env)
        {
            _dataBaseContext = dataBaseContext;
            _env = env;
        }

        public async Task<List<Product>> GetProducts(Guid? category_id)
        {
            var query = _dataBaseContext.Products.AsQueryable();

            if (category_id != null)
            {
                query = query.Where(p => p.CategoryID == category_id.Value);
            }
                
            var products = await query.ToListAsync();

            return products;
        }

        public async Task<List<Product>> GetRandomProducts()
        {
            var randomProducts = await _dataBaseContext.Products
                .Include(p => p.Category)
                .OrderBy(r => EF.Functions.Random())
                .Take(count)
                .ToListAsync();

            return randomProducts;
        }
        public async Task<ProductDto> GetProductById(Guid id)
        {
            var product = await _dataBaseContext.Products
                .Include(p => p.Category)
                .Include(p => p.ProductAttributes)
                .Where(p => p.Id == id)
                .Select(p => new ProductDto(p.Id, p.Name, p.Count, p.Price,
                    new ProductCategoryDto(
                        p.Category.Id,
                        p.Category.Title
                    ),
                    p.ImageUrl, 
                    p.ProductAttributes.Select(pa => new ProductAttributesDto(
                            pa.Id,
                            pa.Title,
                            pa.Content
                    ))
                ))
                .FirstOrDefaultAsync();

            return product;
        }

        public async Task<List<Product>> SearchProductsByNameAsync(string? search, Guid? category_id)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return await GetProducts(category_id);
            }

            //ILike: поиск без учета регистра
            var pattern = $"%{search}%";

            var query = _dataBaseContext.Products.AsQueryable();

            if (category_id != null)
            {
                query = query.Where(p => p.CategoryID == category_id.Value);
            }

            var products = await query
                .Where(p => EF.Functions.ILike(p.Name, pattern))
                .ToListAsync();

            return products;
        }

        public async Task<List<Category>> GetProductCategories()
        {
            var categories = await _dataBaseContext.Categories
                .ToListAsync();

            return categories;
        }

    }
}
