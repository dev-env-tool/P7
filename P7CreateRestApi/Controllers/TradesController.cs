using Microsoft.AspNetCore.Authorization;
//using Dot.Net.WebApi.Controllers.Domain;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Controllers;
using P7CreateRestApi.Domain;
using P7CreateRestApi.IRepositories;

namespace P7CreateRestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TradesController : ControllerBase
    {
        private readonly ITradeRepository _tradeRepository;

        public TradesController(ITradeRepository tradeRepository)
        {
            _tradeRepository = tradeRepository;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllTrades()
        {
            IEnumerable<Trade> listOfTrades = await _tradeRepository.GetAllTrades();
            if (!listOfTrades.Any())
            {
                return NotFound("No information found.");
            }
            return Ok(listOfTrades);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetTradeById(int id)
        {
            IEnumerable<Trade> listOfTrades = await _tradeRepository.GetTradeById(id);

            if (id == 0)
            {
                return BadRequest("Bad request. ID must be an integer and larger than 0.");
            }
            if (!listOfTrades.Any())
            {
                return NotFound("The information with the specified ID was not found.");
            }
            return Ok(listOfTrades);
        }


        [HttpGet]
        [Route("validate")]
        private IActionResult ValidateTradeById([FromBody] Trade trade)
        {
            // TODO: check data valid and save to db, after saving return bid list
            return Ok();
        }


        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateTrade([FromBody] Trade trade)
        {
            // TODO: check required fields, if valid call service to update Bid and return list Bid
            await _tradeRepository.CreateTrade(trade);
            return Ok();
        }



        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateTradeById([FromBody] Trade trade)
        {
            // TODO: check required fields, if valid call service to update Bid and return list Bid
            await _tradeRepository.UpdateTrade(trade);
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteTradeById(int id)
        {
            if (id == 0)
            {
                return BadRequest("Bad request. The information ID must be an integer and larger than 0.");
            }
            if (id > 0)
            {
                await _tradeRepository.DeleteTradeById(id);
                IEnumerable<Trade> listOfTrades = await _tradeRepository.GetTradeById(id);
                if (!listOfTrades.Any())
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