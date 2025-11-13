using System.ComponentModel.DataAnnotations;

namespace ProductsStore.WebAPI.DTO.Cart
{
    public class AddToCartDto
    {
        [Required]
        public Guid ProductId { get; set; }
    }
}
