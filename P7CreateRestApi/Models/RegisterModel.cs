using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P7CreateRestApi.Models
{
    [NotMapped]
    public class RegisterModel
    {
        public string UserName { get; set; }

        [Required(ErrorMessage = "The email is required")]
        [EmailAddress(ErrorMessage = "The email address is not correct")]
        public required string Email { get; set; }
        
        
        //Password validation comes directly from Identity
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = "Admin / Member";
    }
}
