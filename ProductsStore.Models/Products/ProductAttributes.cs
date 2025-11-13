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

        [Column("product_id")]
        public Guid ProductID { get; set; }
        public Product Product { get; set; }
    }
}
