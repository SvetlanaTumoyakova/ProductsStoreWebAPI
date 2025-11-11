using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsStore.Models.Products
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public required string? Title { get; set; }

        [Column("parent_id")]
        public Guid? ParentId { get; set; }
        public Category? Parent { get; set; }
    }
}
