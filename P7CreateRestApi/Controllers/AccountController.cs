using P7CreateRestApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using P7CreateRestApi.Models;

namespace P7CreateRestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllUsers()
        {
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




        [HttpPost("register")]
        //[Route("register")]
        public async Task<IActionResult> CreateUser([FromBody] RegisterModel registerModel)
        {
            User user = new User() {UserName = registerModel.UserName, Email = registerModel.Email, Role = registerModel.Role};
            var result = _userManager.CreateAsync(user, registerModel.Password);
            if (result.IsCompletedSuccessfully)
            {
                return Ok("User was successfully registered.");
            }
            return BadRequest(result.Result);
        }



        [HttpPut]
        [Route("update/generalinfo/{email}")]
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
                userToFind.Role = updateGeneralInfosModel.Role;
                var result = await _userManager.UpdateAsync(userToFind);
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
        [Route("update/password/{email}")]
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