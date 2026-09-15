using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Domain;
using P7CreateRestApi.DTO;
using P7CreateRestApi.Filters;
using P7CreateRestApi.IRepositories;
using P7CreateRestApi.IServices;
using P7CreateRestApi.Models;
using P7CreateRestApi.Services;
using System.Threading.Tasks.Sources;

namespace P7CreateRestApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    
    public class AccountController : Controller
    {
        private readonly IUserService _iUserService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager,
            IUserService userService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _iUserService = userService;
        }

        
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllUsers()
        {
            //IEnumerable<User> listOfUsers = await _userManager.Users();
            IEnumerable<UserDto> listOfUsers = await _iUserService.GetAllUsersDto();
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
            IEnumerable<UserDto> foundUserDto = await _iUserService.GetUserDtoByEmail(email);

            if (email == "")
            {
                return BadRequest("Bad request. UserName must be a character string.");
            }
            if (foundUserDto == null)
            {
                return NotFound("The information with the specified UserName was not found.");
            }
            return Ok(foundUserDto);
        }



        [HttpPost("")]
        [ServiceFilter(typeof(AsyncActionFilter))]
        public async Task<IActionResult> CreateUser([FromBody] RegisterModel registerModel)
        {
            if (!ModelState.IsValid)
            { 
                return BadRequest(ModelState);
            }

            IdentityResult result = await _iUserService.CreateUserWithRegisterModel(registerModel);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return BadRequest(ModelState);
            }
            return Ok("User was successfully registered.");


            //User user = new User() { UserName = registerModel.UserName, Email = registerModel.Email, Role = registerModel.Role };
            //var result = _userManager.CreateAsync(user, registerModel.Password);
            //if (result.Result.Errors.Any())
            //{
            //    return BadRequest(result.Result);
            //}
            //var role = _userManager.AddToRoleAsync(user, registerModel.Role);
            //if (role.Result.Errors.Any())
            //{
            //    return BadRequest(role.Result);
            //}
            //if (ModelState.Any())
            //{
            //    return BadRequest(ModelState);
            //}
            //return Ok("User was successfully registered.");
        }


        [HttpPut]
        [Route("generalinfo/{email}")]
        public async Task<IActionResult> UpdateUserByEmail(string email, [FromBody] UpdateGeneralInfosModel updateGeneralInfosModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            IdentityResult result = await _iUserService.UpdateUserWithUpdateGeneralInfosModel(email, updateGeneralInfosModel);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return BadRequest(ModelState);
            }
            return Ok("User was successfully registered.");





            //if (email == "")
            //{
            //    return BadRequest("Bad request. UserNamemust be a character string.");
            //}
            //User userToFind = await _userManager.FindByEmailAsync(email);


            //if (userToFind == null)
            //{
            //    return NotFound("The information with the specified UserName was not found.");
            //}
            //else
            //{
            //    userToFind.UserName = updateGeneralInfosModel.UserName;
            //    var result = await _userManager.UpdateAsync(userToFind);

            //    if (!await _userManager.IsInRoleAsync(userToFind, updateGeneralInfosModel.Role))
            //    {
            //        if (!await _roleManager.RoleExistsAsync(updateGeneralInfosModel.Role))
            //        {
            //            return BadRequest("Please give one of the followig roles : Admin or Member.");
            //        }
            //        else
            //        {
            //            var roleUpdateResult = await _userManager.AddToRoleAsync(userToFind, updateGeneralInfosModel.Role);
            //            IList<string> foundRoles = await _userManager.GetRolesAsync(userToFind);
            //            foundRoles.Remove(updateGeneralInfosModel.Role);
            //            foreach (var role in foundRoles)
            //            {
            //                await _userManager.RemoveFromRoleAsync(userToFind, role);
            //            }
            //        }
            //    }
            //    if (result.Succeeded)
            //    {
            //        return Ok(result.Succeeded);
            //    }
            //    else
            //    {
            //        return BadRequest(result.Errors);
            //    }


            //if (email == "")
            //{
            //    return BadRequest("Bad request. UserNamemust be a character string.");
            //}
            //User userToFind = await _userManager.FindByEmailAsync(email);


            //if (userToFind == null)
            //{
            //    return NotFound("The information with the specified UserName was not found.");
            //}
            //else
            //{
            //    userToFind.UserName = updateGeneralInfosModel.UserName;
            //    var result = await _userManager.UpdateAsync(userToFind);

            //    if (!await _userManager.IsInRoleAsync(userToFind, updateGeneralInfosModel.Role))
            //    {
            //        if (!await _roleManager.RoleExistsAsync(updateGeneralInfosModel.Role))
            //        {
            //            return BadRequest("Please give one of the followig roles : Admin or Member.");
            //        }
            //        else
            //        {
            //            var roleUpdateResult = await _userManager.AddToRoleAsync(userToFind, updateGeneralInfosModel.Role);
            //            IList<string> foundRoles = await _userManager.GetRolesAsync(userToFind);
            //            foundRoles.Remove(updateGeneralInfosModel.Role);
            //            foreach (var role in foundRoles)
            //            {
            //                await _userManager.RemoveFromRoleAsync(userToFind, role);
            //            }
            //        }
            //    }
            //    if (result.Succeeded)
            //    {
            //        return Ok(result.Succeeded);
            //    }
            //    else
            //    {
            //        return BadRequest(result.Errors);
            //    }
            //}

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
                if (result.Errors.Any())
                {
                    return BadRequest(result.Errors);
                }
                else
                {
                    return Ok("User was successfully deleted");
                }
            }
        }
    }
}