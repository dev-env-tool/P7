
//using Microsoft.AspNetCore.Mvc;
//using P7CreateRestApi.Domain;
//using P7CreateRestApi.IRepositories;
//using P7CreateRestApi.Repositories;

//namespace Dot.Net.WebApi.Controllers
//{
//    [ApiController]
//    [Route("[controller]")]
//    public class UsersController : ControllerBase
//    {
//        private readonly UserRepository _userRepository;

//        public UsersController(UserRepository userRepository)
//        {
//            _userRepository = userRepository;
//        }

//        [HttpGet]
//        [Route("")]
//        public IActionResult Home()
//        {
//            return Ok();
//        }

//        [HttpGet]
//        [Route("add")]
//        public IActionResult AddUser([FromBody] User user)
//        {
//            return Ok();
//        }

//        [HttpGet]
//        [Route("validate")]
//        public IActionResult Validate([FromBody] User user)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest();
//            }

//            _userRepository.CreateUser(user);

//            return Ok();
//        }

//        [HttpGet]
//        [Route("{id}")]
//        public IActionResult ShowUpdateForm(int id)
//        {
//            User user = _userRepository.GetUserById(id);

//            if (user == null)
//                throw new ArgumentException("Invalid user Id:" + id);

//            return Ok();
//        }

//        [HttpPost]
//        [Route("{id}")]
//        public IActionResult UpdateUser(int id, [FromBody] User user)
//        {
//            // TODO: check required fields, if valid call service to update Trade and return Trade list
//            return Ok();
//        }

//        [HttpDelete]
//        [Route("{id}")]
//        public IActionResult DeleteUser(int id)
//        {
//            User user = _userRepository.FindById(id);

//            if (user == null)
//                throw new ArgumentException("Invalid user Id:" + id);

//            return Ok();
//        }

//        [HttpGet]
//        [Route("/secure/article-details")]
//        public async Task<ActionResult<List<User>>> GetAllUserArticles()
//        {
//            return Ok();
//        }
//    }
//}


using P7CreateRestApi.Domain;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.IRepositories;
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
            IEnumerable<IdentityUser> listOfUsers = _userManager.Users;
            if (!listOfUsers.Any())
            {
                return NotFound("No information found.");
            }
            return Ok(listOfUsers);
        }

        //[HttpGet]
        //[Route("{email}")]
        //public async Task<IActionResult> GetUserByEmail(string email)
        //{
        //    IEnumerable<User> listOfUsers = await _userRepository.GetUserByEmail(email);

        //    if (email == "")
        //    {
        //        return BadRequest("Bad request. UserName must be a character string.");
        //    }
        //    if (!listOfUsers.Any())
        //    {
        //        return NotFound("The information with the specified UserName was not found.");
        //    }
        //    return Ok(listOfUsers);
        //}


        [HttpGet("validate")]
        //[Route("validate")]
        private IActionResult ValidateUserById([FromBody] User user)
        {
            // TODO: check data valid and save to db, after saving return bid list
            return Ok();
        }


        [HttpPost("register")]
        //[Route("register")]
        public async Task<IActionResult> CreateUser([FromBody] RegisterModel registerModel)
        {
            // TODO: check required fields, if valid call service to update Bid and return list Bid
            User user = new User() {UserName = registerModel.UserName, Email = registerModel.Email, Role = registerModel.Role};
            //await _userManager.CreateAsync(user, registerModel.Password);
            //return Ok("okok");
            var result = _userManager.CreateAsync(user, registerModel.Password);
            if (result.IsCompletedSuccessfully)
            {
                return Ok("User was successfully registered.");
            }
            return BadRequest(result.Result);
        }



        [HttpPut]
        [Route("userName")]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            // TODO: check required fields, if valid call service to update Bid and return list Bid
            if (user.Email == "")
            {
                return BadRequest("Bad request. UserNamemust be a character string.");
            }
            //if (GetUserByEmail(user.Email) == null)
            //{
            //    return NotFound("The information with the specified UserName was not found.");
            //}
            else
            {
                await _userManager.UpdateAsync(user);
                return Ok();
            }
        }

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
                //User user = await _userManager.GetUserAsync(em);
                //await _userManager.DeleteAsync();
                return Ok("The item was deleted with success.");
            }
        }
    }
}