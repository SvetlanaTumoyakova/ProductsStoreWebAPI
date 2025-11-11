using System.ComponentModel.DataAnnotations;

namespace ProductsStore.WebAPI.DTO
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string UserName { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
