using System.ComponentModel.DataAnnotations.Schema;

namespace P7CreateRestApi.Models
{
    [NotMapped]
    public class UpdateGeneralInfosModel
    {
        public string Role { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }
}
