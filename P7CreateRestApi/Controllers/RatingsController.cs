using Dot.Net.WebApi.Controllers.Domain;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.IRepositories;
using System.Collections;
using System.Diagnostics;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingsController : ControllerBase
    {
        private IRatingRepository _RatingRepository;

        public RatingsController(IRatingRepository RatingRepository)
        {
            _RatingRepository = RatingRepository;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllRatings()
        {
            IEnumerable<Rating> listOfRatings = await _RatingRepository.GetAllRatings();
            if (listOfRatings.Count() == 0)
            {
                return NotFound("No information found.");
            }
            return Ok(listOfRatings);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetRatingById(int id)
        {
            IEnumerable<Rating> listOfRatings = await _RatingRepository.GetRatingById(id);

            if (id == 0)
            {
                return BadRequest("Bad request. ID must be an integer and larger than 0.");
            }
            if (listOfRatings.Count() == 0)
            {
                return NotFound("A user with the specified ID was not found.");
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
        public IActionResult CreateRating(int id, [FromBody] Rating rating)
        {
            // TODO: check required fields, if valid call service to update Bid and return list Bid
            _RatingRepository.CreateRating(rating);
            return Ok();
        }



        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateRatingById(int id, [FromBody] Rating rating)
        {
            // TODO: check required fields, if valid call service to update Bid and return list Bid
            await _RatingRepository.UpdateRating(rating);
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteRating(int id)
        {
            if (id == 0)
            {
                return BadRequest("Bad request. User ID must be an integer and larger than 0.");
            }
            if (id > 0)
            {
                _RatingRepository.DeleteRatingById(id);
                IEnumerable<Rating> listOfRatings = await _RatingRepository.GetRatingById(id);
                if (listOfRatings.Count() == 0)
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