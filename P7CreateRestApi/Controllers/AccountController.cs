using P7CreateRestApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using P7CreateRestApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace P7CreateRestApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    
    public class AccountController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllUsers()
        {
            bool test = User.Identity.IsAuthenticated;
            string test2 = User.Identity.AuthenticationType;
            //IEnumerable<User> listOfUsers = await _userManager.Users();
            IEnumerable<User> listOfUsers = _userManager.Users;
            if (!listOfUsers.Any())
            {
                return NotFound("No information found.");
            }
            return Ok(listOfUsers);
        }

        [HttpGet]
        [Route("{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            bool test = System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.DefaultMapInboundClaims;
            User foundUser = await _userManager.FindByEmailAsync(email);

            if (email == "")
            {
                return BadRequest("Bad request. UserName must be a character string.");
            }
            if (foundUser == null)
            {
                return NotFound("The information with the specified UserName was not found.");
            }
            return Ok(foundUser);
        }




        [HttpPost("")]
        //[Route("register")]
        public async Task<IActionResult> CreateUser([FromBody] RegisterModel registerModel)
        {
            User user = new User() {UserName = registerModel.UserName, Email = registerModel.Email, Role = registerModel.Role};
            var result = _userManager.CreateAsync(user, registerModel.Password);
            if (result.Result.Errors.Any())
            {
                return BadRequest(result.Result); 
            }
            var role = _userManager.AddToRoleAsync(user, registerModel.Role);
            if (role.Result.Errors.Any())
            {
                return BadRequest(role.Result);
            }
            return Ok("User was successfully registered.");
        }



        [HttpPut]
        [Route("generalinfo/{email}")]
        public async Task<IActionResult> UpdateUserByEmail(string email, [FromBody] UpdateGeneralInfosModel updateGeneralInfosModel)
        {
            if (email == "")
            {
                return BadRequest("Bad request. UserNamemust be a character string.");
            }
            User userToFind = await _userManager.FindByEmailAsync(email);
            

            if (userToFind == null)
            {
                return NotFound("The information with the specified UserName was not found.");
            }
            else
            {
                userToFind.UserName = updateGeneralInfosModel.UserName;
                var result = await _userManager.UpdateAsync(userToFind);

                if (!await _userManager.IsInRoleAsync(userToFind, updateGeneralInfosModel.Role))
                {
                    if(!await _roleManager.RoleExistsAsync(updateGeneralInfosModel.Role))
                    {
                        return BadRequest("Please give one of the followig roles : Admin or Member.");
                    }
                    else
                    { 
                        var roleUpdateResult = await _userManager.AddToRoleAsync(userToFind, updateGeneralInfosModel.Role);
                        IList<string> foundRoles = await _userManager.GetRolesAsync(userToFind);
                        foundRoles.Remove(updateGeneralInfosModel.Role);
                        foreach (var role in foundRoles)
                        {
                            await _userManager.RemoveFromRoleAsync(userToFind, role);
                        }
                    }
                }
                if (result.Succeeded)
                { 
                    return Ok(result.Succeeded);
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
        }


        [HttpPut]
        [Route("password/{email}")]
        public async Task<IActionResult> UpdateUserPasswordByEmail(string email, [FromBody] UpdatePasswordModel updatePasswordModel)
        {
            if (email == "")
            {
                return BadRequest("Bad request. UserNamemust be a character string.");
            }
            User userToFind = await _userManager.FindByEmailAsync(email);


            if (userToFind == null)
            {
                return NotFound("The information with the specified UserName was not found.");
            }
            else
            {
                var result = await _userManager.ChangePasswordAsync(userToFind, updatePasswordModel.CurrentPassword, updatePasswordModel.NewPassword);
                if (result.Succeeded)
                {
                    return Ok(result.Succeeded);
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
        }

       


        //[HttpGet("validate")]
        ////[Route("validate")]
        //private IActionResult ValidateUserById([FromBody] User user)
        //{
        //    // TODO: check data valid and save to db, after saving return bid list
        //    return Ok();
        //}


        [HttpDelete]
        [Route("{email}")]
        public async Task<IActionResult> DeleteUserByEmail(string email)
        {

            if (email == "")
            {
                return BadRequest("Bad request. UserName must be a character string.");
            }
            else
            {
                User user = await _userManager.FindByEmailAsync(email);
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return Ok(result.Succeeded);
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
        }
    }
}