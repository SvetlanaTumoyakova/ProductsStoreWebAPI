using System.ComponentModel.DataAnnotations;

namespace ProductsStore.WebAPI.DTO.Cart
{
    public class DeleteFromCartDto
    {
        [Required]
        public Guid ProductId { get; set; }
    }
}
