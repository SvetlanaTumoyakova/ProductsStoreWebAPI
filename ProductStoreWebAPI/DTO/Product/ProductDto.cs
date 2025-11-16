using ProductsStore.Models.Orders;
using ProductsStore.Models.Products;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProductsStore.WebAPI.DTO.Product
{
    public record ProductDto
    (
        Guid Id,
        string? Name,
        int? Count,
        double? Price,
        ProductCategoryDto Category,
        string? ImageUrl,
        IEnumerable<ProductAttributesDto> ProductAttributes
    );
}

