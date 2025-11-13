using System.ComponentModel.DataAnnotations;

namespace ProductsStore.WebAPI.DTO.Product
{
    public class CreateProductDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        public double Price { get; set; }

        [Required]
        public Guid CategoryId { get; set; }
    }
}
