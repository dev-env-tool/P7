namespace P7CreateRestApi.Models
{
    public class UpdatePasswordModel
    {
        public string CurrentPassword { get; set; } = string.Empty;

        public string NewPassword { get; set; } = string.Empty;
    }
}
