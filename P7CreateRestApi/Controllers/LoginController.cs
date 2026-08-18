using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace P7CreateRestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]


    public class LoginController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _iConfiguration;
        
        public LoginController(SignInManager<User> signInManager, UserManager<User> userManager, IConfiguration iConfiguration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _iConfiguration = iConfiguration;
        }

        

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            //TODO: implement the UserManager from Identity to validate User and return a security token.
            var result = await _signInManager.PasswordSignInAsync(loginModel.UserName, loginModel.Password, true, false);
            if (result.Succeeded)
            {
                IEnumerable<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Email, loginModel.UserName),
                    new Claim(ClaimTypes.Role,"Admin"),
                };
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_iConfiguration.GetSection("Jwt:Key").Value));
                SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

                JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(15),
                    issuer: _iConfiguration.GetSection("Jwt:Issuer").Value,
                    audience: _iConfiguration.GetSection("Jwt:Audience").Value,
                    signingCredentials: signingCredentials);

                var tokenString = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                return Ok(tokenString);
            }
            return BadRequest(result.IsNotAllowed);
        }



        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            //TODO: implement the UserManager from Identity to validate User and return a security token.
            var result = _signInManager.SignOutAsync();
            if (result.IsCompletedSuccessfully)
            {
                return Ok("Logged out");
            }
            return BadRequest("Logout failed");
        }
    }
}
