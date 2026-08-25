using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Domain;
using P7CreateRestApi.IRepositories;
using Serilog;

namespace P7CreateRestApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BidListsController : ControllerBase
    {

        
        private readonly IBidListRepository _bidListRepository;

        public BidListsController(IBidListRepository bidListRepository)
        {
            _bidListRepository = bidListRepository;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllBidLists()
        {
            IEnumerable<BidList> listOfBidLists = await _bidListRepository.GetAllBidLists();
            if (!listOfBidLists.Any())
            {
                return NotFound("No information found.");
            }
            //Log.Information("{UserName} at {Now}",Serilog.Context.LogContext. , DateTime.Now);
            return Ok(listOfBidLists);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetBidListById(int id)
        {
            IEnumerable<BidList> listOfBidLists = await _bidListRepository.GetBidListById(id);

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
        private IActionResult ValidateBidListById([FromBody] BidList BidList)
        {
            // TODO: check data valid and save to db, after saving return bid list
            return Ok();
        }


        [HttpPost]
        [Route("")]
        public async Task <IActionResult> CreateBidList([FromBody] BidList BidList)
        {
            // TODO: check required fields, if valid call service to update Bid and return list Bid
            await _bidListRepository.CreateBidList(BidList);
            return Ok("Item was successfully created");
        }



        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateBidListById([FromBody] BidList BidList)
        {
            // TODO: check required fields, if valid call service to update Bid and return list Bid
            await _bidListRepository.UpdateBidList(BidList);
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteBidListById(int id)
        {
            if (id == 0)
            {
                return BadRequest("Bad request. User ID must be an integer and larger than 0.");
            }
            if (id > 0)
            {
                await _bidListRepository.DeleteBidListById(id);
                IEnumerable<BidList> listOfBidLists = await _bidListRepository.GetBidListById(id);
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