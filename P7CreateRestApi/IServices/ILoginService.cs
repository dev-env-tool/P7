using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Models;

namespace P7CreateRestApi.IServices
{
    public interface ILoginService
    {
        Task<Microsoft.AspNetCore.Identity.SignInResult> Login([FromBody] LoginModel loginModel);
        Task<bool> Logout();
        Task<string> GenerateTokenString(LoginModel loginModel);
    }
}