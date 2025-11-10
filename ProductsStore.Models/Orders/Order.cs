using ProductStoreWebAPI.Model.Products;
using ProductStoreWebAPI.Model.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductStoreWebAPI.Model.Orders
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Column("user_id")]
        public Guid UserID { get; set; }

        public required User User { get; set; }

        public required ICollection<Product> Products { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
