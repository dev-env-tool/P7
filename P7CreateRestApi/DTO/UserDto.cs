using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace P7CreateRestApi.DTO
{
    [BindNever]
    public class UserDto
    {
        //public string Id { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        //public string Fullname { get; set; }

    }
}
