using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Domain;
using P7CreateRestApi.DTO;
using P7CreateRestApi.IRepositories;
using P7CreateRestApi.IServices;
using P7CreateRestApi.Models;
using P7CreateRestApi.Services;

namespace P7CreateRestApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    
    public class AccountController : Controller
    {
        private readonly IUserRepository _iUserRepository;
        private readonly IUserService _iUserService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager,
            IUserRepository userRepository, IUserService userService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _iUserRepository = userRepository;
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
        //[Route("register")]
        public async Task<IActionResult> CreateUser([FromBody] UserDto userDto)
        {
            await _iUserService.CreateUserWithUserDto(userDto);
            return Ok("User was successfully registered.");
        }



        [HttpPut]
        [Route("generalinfo/{email}")]
        public async Task<IActionResult> UpdateUserByEmail(string email, [FromBody] UpdateGeneralInfosModel updateGeneralInfosModel)
        {
            if (email == "")
            {
                return BadRequest("Bad request. Please give an email.");
            }

            var result = _iUserService.UpdateUserWithUpdateGeneralInfosModel(email, updateGeneralInfosModel);

            if (result.IsCompleted)
            { 
                return Ok(result.IsCompleted);
            }
            else
            {
                return BadRequest(result.IsFaulted);
            }
        }


        [HttpPut]
        [Route("password/{email}")]
        public async Task<IActionResult> UpdateUserPasswordByEmail(string email, [FromBody] UpdatePasswordModel updatePasswordModel)
        {
            if (email == "")
            {
                return BadRequest("Bad request. Please give an email.");
            }

            var result = _iUserService.UpdateUserPasswordWithUpdatePasswordModel(email, updatePasswordModel);

            if (result.IsCompleted)
            {
                return Ok(result.IsCompleted);
            }
            else
            {
                return BadRequest(result.IsFaulted);
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