using ProductsStore.Models.Carts;
using ProductStoreWebAPI.Model.Orders;
using ProductStoreWebAPI.Model.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProductStoreWebAPI.Model.Products
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public required string? Name { get; set; }

        public required int? Count { get; set; }

        public required double? Price { get; set; }

        [Column("categoty_id")]
        public Guid CategoryID { get; set; }
        public required Category Category { get; set; }

        [JsonIgnore]
        public virtual ICollection<ProductAttribute> ProductAttributes { get; set; } = new List<ProductAttribute>();

        [JsonIgnore]
        public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();
        [JsonIgnore]
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
