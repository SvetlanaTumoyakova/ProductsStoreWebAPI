using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsStore.Models.Users
{
    public class User
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string? UserName { get; set; }

        [Required]
        [MinLength(6)]
        public required string? PasswordHash { get; set; }

        [Column("person_id")]
        public Guid PersonID { get; set; }

        public required Person Person { get; set; }

        [Column("role_id")]
        public Guid RoleID { get; set; }

        public required UserRole UserRole { get; set; }

    }
}
