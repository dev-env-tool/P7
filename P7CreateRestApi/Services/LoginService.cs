using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using P7CreateRestApi.Domain;
using P7CreateRestApi.IServices;
using P7CreateRestApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace P7CreateRestApi.Services
{
    public class LoginService : ILoginService

    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _iConfiguration;


        public LoginService(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration iConfiguration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _iConfiguration = iConfiguration;
        }

        //public async Task<bool> Login(LoginModel loginModel)
        //{
        //    var result = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, true, false);
        //    if (result.Succeeded)
        //    {
        //        return (result.Succeeded);
        //    }
        //    return false;

        //}
        public async Task<Microsoft.AspNetCore.Identity.SignInResult> Login(LoginModel loginModel)
        {
            var result2 = await _userManager.FindByEmailAsync(loginModel.Email);
            var ok = await _userManager.CheckPasswordAsync(result2, result2.Password);

            var result = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, true, false);

            if (result.Succeeded)
            {
                return result;
            }
            return Microsoft.AspNetCore.Identity.SignInResult.NotAllowed;

        }


        public async Task<bool> Logout()
        {
            //TODO: implement the UserManager from Identity to validate User and return a security token.
            var result = _signInManager.SignOutAsync();
            if (result.IsCompletedSuccessfully)
            {
                return result.IsCompletedSuccessfully;
            }
            return false;
        }

        public async Task<string> GenerateTokenString(LoginModel loginModel)
        {
            User user = await _userManager.FindByEmailAsync(loginModel.Email);
            string role = "Member";
            bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            if (isAdmin == true)
            {
                role = "Admin";
            }

            IEnumerable<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, loginModel.Email),
                new Claim(ClaimTypes.Role,role),
            };
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_iConfiguration.GetSection("Jwt:Key").Value));
            //SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.Aes256CbcHmacSha512);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                issuer: _iConfiguration.GetSection("Jwt:Issuer").Value,
                audience: _iConfiguration.GetSection("Jwt:Audience").Value,
                signingCredentials: signingCredentials);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            //public override string WriteToken(Microsoft.IdentityModel.Tokens.SecurityToken token);
            //var tokenString = new JwtSecurityTokenHandler().CreateToken(jwtSecurityToken);
            return tokenString;
        }


    }
}
