using System.ComponentModel.DataAnnotations.Schema;

namespace P7CreateRestApi.Models
{
    [NotMapped]
    public class LoginModel
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
