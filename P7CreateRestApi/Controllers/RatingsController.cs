using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Filters;
using P7CreateRestApi.IRepositories;
using P7CreateRestApi.DTO;
using AutoMapper;
using P7CreateRestApi.IServices;

namespace P7CreateRestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IRatingService _ratingService;

        public RatingsController(IRatingRepository ratingRepository, IRatingService ratingService)
        {
            _ratingRepository = ratingRepository;
            _ratingService = ratingService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllRatings()
        {
            IEnumerable<RatingDto> listOfRatings = await _ratingService.GetAllRatingsDto();
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
            IEnumerable<RatingDto> listOfRatings = await _ratingService.GetRatingDtoById(id);

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
        private IActionResult ValidateRatingById([FromBody] RatingDto rating)
        {
            // TODO: check data valid and save to db, after saving return bid list
            return Ok();
        }


        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateRating([FromBody] RatingDto ratingDto)
        {
            await _ratingService.CreateRatingWithRatingDto(ratingDto);
            return Ok();
        }



        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateRatingById([FromBody] RatingDto ratingDto)
        {
            await _ratingService.UpdateRatingWithRatingDto(ratingDto);
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
                await _ratingRepository.DeleteRatingById(id);
                IEnumerable<RatingDto> listOfRatings = await _ratingService.GetRatingDtoById(id);
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