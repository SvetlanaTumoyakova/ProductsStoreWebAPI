using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsStore.Models.Products
{
    public class ProductAttributes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public required string? Title { get; set; }
        public required string? Content { get; set; }

        public required ICollection<Product> Products { get; set; }
    }
}
