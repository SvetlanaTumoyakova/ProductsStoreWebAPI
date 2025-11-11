using System.ComponentModel.DataAnnotations;

namespace ProductsStore.WebAPI.DTO
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string UserName { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required, MaxLength(100)]
        public string LastName { get; set; }

        [Required, MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string? Patronymic { get; set; }

        [MaxLength(350)]
        public string? Address { get; set; }

        [MaxLength(50)]
        public string? Phone { get; set; }


    }
}
