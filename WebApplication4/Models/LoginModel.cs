using System.ComponentModel.DataAnnotations;
#nullable disable
namespace WebApplication4.Models
{
    public class LoginModel
    {
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
    }
}
