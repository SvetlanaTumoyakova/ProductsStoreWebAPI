using Microsoft.AspNetCore.Mvc;
using ProductsStore.DAL;
using ProductsStore.WebAPI.Providers;

namespace ProductsStore.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly DataBaseContext _context;
        private readonly ProductProvider _productProvider;

        public ProductController(DataBaseContext context, ProductProvider productProvider)
        {
            _context = context;
            _productProvider = productProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery(Name = "category_id")] Guid? category_id)
        {
            var products = await _productProvider.GetProducts(category_id);
            return Ok(products);
        }

        [HttpGet("random")]
        public async Task<IActionResult> GetRandom()
        {
            var randomProducts = await _productProvider.GetRandomProducts();
            return Ok(randomProducts);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var product = await _productProvider.GetProductById(id);

            if (product == null) return NotFound();

            return Ok(product);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery(Name = "q")] string? q, [FromQuery(Name = "category_id")] Guid? category_id)
        {
            var products = await _productProvider.SearchProductsByNameAsync(q, category_id);
            return Ok(products);
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var productCategories = await _productProvider.GetProductCategories();
            return Ok(productCategories);
        }

    }
}
