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
    public class BidListsController : ControllerBase
    {
        private IBidListRepository _bidListRepository;

        public BidListsController(IBidListRepository bidListRepository)
        {
            _bidListRepository = bidListRepository;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllBidLists()
        {
            IEnumerable<BidList> listOfBidLists = await _bidListRepository.GetAllBidLists();
            if (listOfBidLists.Count() == 0)
            {
                return NotFound("No information found.");
            }
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
            if (listOfBidLists.Count() == 0)
            {
                return NotFound("A user with the specified ID was not found.");
            }
            return Ok(listOfBidLists);
        }


        [HttpGet]
        [Route("validate")]
        private IActionResult ValidateBidListById([FromBody] BidList bidList)
        {
            // TODO: check data valid and save to db, after saving return bid list
            return Ok();
        }


        [HttpPost]
        [Route("")]
        public IActionResult CreateBidList(int id, [FromBody] BidList bidList)
        {
            // TODO: check required fields, if valid call service to update Bid and return list Bid
            return Ok();
        }



        [HttpPut]
        [Route("{id}")]
        public IActionResult UpdateBidListById(int id, [FromBody] BidList bidList)
        {
            // TODO: check required fields, if valid call service to update Bid and return list Bid
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteBidList(int id)
        {
            if (id == 0)
            {
                return BadRequest("Bad request. User ID must be an integer and larger than 0.");
            }
            if (id > 0)
            { 
                _bidListRepository.DeleteBidListById(id);
                IEnumerable<BidList> listofBidLists = await _bidListRepository.GetBidListById(id);
                if (listofBidLists.Count() == 0)
                {
                    return Ok("The item was deleted with success.");
                }
                else
                {
                    return StatusCode(500, "Unexpected error happened.");
                }
            }
            return StatusCode(500,"Unexpected error happened.");
        }
    }
}