using ProductsStore.Models.Carts;
using ProductsStore.Models.Orders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProductsStore.Models.Products
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public required string? Name { get; set; }

        public required int? Count { get; set; }

        public required double? Price { get; set; }

        [Column("category_id")]
        public Guid CategoryID { get; set; }
        public required Category Category { get; set; }

        //пример, "/static/filename.jpg"
        [Column("image_url")]
        public string? ImageUrl { get; set; }

        public virtual ICollection<ProductAttributes> ProductAttributes { get; set; } = new List<ProductAttributes>();

        [JsonIgnore]
        public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();
        [JsonIgnore]
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
