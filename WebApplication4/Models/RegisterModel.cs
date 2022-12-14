using System.ComponentModel.DataAnnotations;
#nullable disable
namespace WebApplication4.Models
{
    public class RegisterModel
    {
        [Display(Name = "Enter name")]
        [Required]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        [Display(Name = "Enter email")]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Display(Name = "Enter password")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Repeat password")]
        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]  
        public string RepeatPassword { get; set; }

    }
}
