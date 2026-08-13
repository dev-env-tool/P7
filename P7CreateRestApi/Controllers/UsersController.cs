
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

namespace P7CreateRestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {


        private readonly IUserRepository _UserRepository;

        public UsersController(IUserRepository UserRepository)
        {
            _UserRepository = UserRepository;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllUsers()
        {
            IEnumerable<User> listOfUsers = await _UserRepository.GetAllUsers();
            if (!listOfUsers.Any())
            {
                return NotFound("No information found.");
            }
            return Ok(listOfUsers);
        }

        [HttpGet]
        [Route("{userName}")]
        public async Task<IActionResult> GetUserByUserName(string userName)
        {
            IEnumerable<User> listOfUsers = await _UserRepository.GetUserByUserName(userName);

            if (userName == "")
            {
                return BadRequest("Bad request. UserName must be a character string.");
            }
            if (!listOfUsers.Any())
            {
                return NotFound("The information with the specified UserName was not found.");
            }
            return Ok(listOfUsers);
        }


        [HttpGet]
        [Route("validate")]
        private IActionResult ValidateUserById([FromBody] User User)
        {
            // TODO: check data valid and save to db, after saving return bid list
            return Ok();
        }


        [HttpPost]
        [Route("")]
        public IActionResult CreateUser([FromBody] User User)
        {
            // TODO: check required fields, if valid call service to update Bid and return list Bid
            _UserRepository.CreateUser(User);
            return Ok();
        }



        [HttpPut]
        [Route("userName")]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            // TODO: check required fields, if valid call service to update Bid and return list Bid
            if (user.UserName == "")
            {
                return BadRequest("Bad request. UserNamemust be a character string.");
            }
            if (GetUserByUserName(user.UserName) == null)
            {
                return NotFound("The information with the specified UserName was not found.");
            }
            else
            {
                await _UserRepository.UpdateUser(user);
                return Ok();
            }
        }

        [HttpDelete]
        [Route("{userName}")]
        public async Task<IActionResult> DeleteUserByUserName(string userName)
        {
            if (userName == "")
            {
                return BadRequest("Bad request. UserName must be a character string.");
            }
            else
            {
                _UserRepository.DeleteUserByUserName(userName);
                if (GetUserByUserName(userName) == null)
                {
                    return Ok("The item was deleted with success.");
                }
                else
                {
                    return StatusCode(500, "Unexpected error happened.");
                }
            }
        }
    }
}