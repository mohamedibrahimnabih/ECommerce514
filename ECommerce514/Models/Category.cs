using ECommerce514.Validation;
using System.ComponentModel.DataAnnotations;

namespace ECommerce514.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        [CustomLength(20)]
        public string? Description { get; set; }
        public bool Status { get; set; }

        public ICollection<Product> Products { get; } = new List<Product>();
    }
}
