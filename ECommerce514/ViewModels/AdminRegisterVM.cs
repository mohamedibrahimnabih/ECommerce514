using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ECommerce514.ViewModels
{
    public class AdminRegisterVM
    {
        public string Id { get; set; }
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string? Address { get; set; }
        public List<SelectListItem> Roles { get; set; }
    }
}
