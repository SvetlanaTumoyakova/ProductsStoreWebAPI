using ProductStoreWebAPI.Model.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProductStoreWebAPI.Model.Products;

namespace ProductsStore.Models.Carts
{
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Column("user_id")]
        public Guid UserID { get; set; }

        public required User User { get; set; }

        public required ICollection<Product> Products { get; set; }
    }
}
