using Microsoft.AspNetCore.Identity;



namespace P7CreateRestApi.Domain
{
    public class User : IdentityUser
    {
        public override string Id { get; set; } = string.Empty;
        public override string UserName { get; set; }
        public string Password { get; set; } = string.Empty;
        public string Fullname { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}