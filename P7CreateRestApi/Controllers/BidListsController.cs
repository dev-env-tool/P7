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
    public class BidListsController : ControllerBase
    {
        private readonly IBidListRepository _bidListRepository;
        private readonly IBidListService _bidListService;

        public BidListsController(IBidListRepository bidListRepository, IBidListService bidListService)
        {
            _bidListRepository = bidListRepository;
            _bidListService = bidListService;
        }

        [Authorize(Roles = "Member, Admin")]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllBidLists()
        {
            IEnumerable<BidListDto> listOfBidLists = await _bidListService.GetAllBidListsDto();
            if (!listOfBidLists.Any())
            {
                return NotFound("No information found.");
            }
            return Ok(listOfBidLists);
        }

        [Authorize(Roles = "Member, Admin")]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetBidListById(int id)
        {
            IEnumerable<BidListDto> listOfBidLists = await _bidListService.GetBidListDtoById(id);

            if (id == 0)
            {
                return BadRequest("Bad request. ID must be an integer and larger than 0.");
            }
            if (!listOfBidLists.Any())
            {
                return NotFound("The information with the specified ID was not found.");
            }
            return Ok(listOfBidLists);
        }


        [HttpGet]
        [Route("validate")]
        private IActionResult ValidateBidListById([FromBody] BidListDto BidList)
        {
            // TODO: check data valid and save to db, after saving return bid list
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [ServiceFilter(typeof(AsyncActionFilter))]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateBidList([FromBody] BidListDto BidListDto)
        {
            await _bidListService.CreateBidListWithBidListDto(BidListDto);
            return Ok();
        }


        [Authorize(Roles = "Admin")]
        [ServiceFilter(typeof(AsyncActionFilter))]
        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateBidListById([FromBody] BidListDto BidListDto)
        {
            await _bidListService.UpdateBidListWithBidListDto(BidListDto);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteBidListById(int id)
        {
            if (id == 0)
            {
                return BadRequest("Bad request. The information ID must be an integer and larger than 0.");
            }
            if (id > 0)
            {
                await _bidListRepository.DeleteBidListById(id);
                IEnumerable<BidListDto> listOfBidLists = await _bidListService.GetBidListDtoById(id);
                if (!listOfBidLists.Any())
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