using System.ComponentModel.DataAnnotations;

namespace ECommerce514.ViewModels
{
    public class ForgetPasswordVM
    {
        public int Id { get; set; }
        [Required]
        public string EmailOrUserName { get; set; }
    }
}
