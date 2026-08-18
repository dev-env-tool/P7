using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Domain;
using P7CreateRestApi.IRepositories;

namespace P7CreateRestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingRepository _ratingRepository;

        public RatingsController(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllRatings()
        {
            IEnumerable<Rating> listOfRatings = await _ratingRepository.GetAllRatings();
            if (!listOfRatings.Any())
            {
                return NotFound("No information found.");
            }
            return Ok(listOfRatings);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetRatingById(int id)
        {
            IEnumerable<Rating> listOfRatings = await _ratingRepository.GetRatingById(id);

            if (id == 0)
            {
                return BadRequest("Bad request. ID must be an integer and larger than 0.");
            }
            if (!listOfRatings.Any())
            {
                return NotFound("The information with the specified ID was not found.");
            }
            return Ok(listOfRatings);
        }


        [HttpGet]
        [Route("validate")]
        private IActionResult ValidateRatingById([FromBody] Rating rating)
        {
            // TODO: check data valid and save to db, after saving return bid list
            return Ok();
        }


        [HttpPost]
        [Route("")]
        public IActionResult CreateRating([FromBody] Rating rating)
        {
            // TODO: check required fields, if valid call service to update Bid and return list Bid
            _ratingRepository.CreateRating(rating);
            return Ok();
        }



        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateRatingById([FromBody] Rating rating)
        {
            // TODO: check required fields, if valid call service to update Bid and return list Bid
            await _ratingRepository.UpdateRating(rating);
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteRatingById(int id)
        {
            if (id == 0)
            {
                return BadRequest("Bad request. The information ID must be an integer and larger than 0.");
            }
            if (id > 0)
            {
                _ratingRepository.DeleteRatingById(id);
                IEnumerable<Rating> listOfRatings = await _ratingRepository.GetRatingById(id);
                if (!listOfRatings.Any())
                {
                    return Ok("The item was deleted with success.");
                }
                else
                {
                    return StatusCode(500, "Unexpected error happened.");
                }
            }
            return StatusCode(500, "Unexpected error happened.");
        }
    }
}