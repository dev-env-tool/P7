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
    public class TradesController : ControllerBase
    {
        private readonly ITradeRepository _TradeRepository;
        private readonly ITradeService _TradeService;

        public TradesController(ITradeRepository TradeRepository, ITradeService TradeService)
        {
            _TradeRepository = TradeRepository;
            _TradeService = TradeService;
        }

        [Authorize(Roles = "Member, Admin")]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllTrades()
        {
            IEnumerable<TradeDto> listOfTrades = await _TradeService.GetAllTradesDto();
            if (!listOfTrades.Any())
            {
                return NotFound("No information found.");
            }
            return Ok(listOfTrades);
        }

        [Authorize(Roles = "Member, Admin")]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetTradeById(int id)
        {
            IEnumerable<TradeDto> listOfTrades = await _TradeService.GetTradeDtoById(id);

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
        private IActionResult ValidateTradeById([FromBody] TradeDto Trade)
        {
            // TODO: check data valid and save to db, after saving return bid list
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [ServiceFilter(typeof(AsyncActionFilter))]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateTrade([FromBody] TradeDto TradeDto)
        {
            await _TradeService.CreateTradeWithTradeDto(TradeDto);
            return Ok();
        }


        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateTradeById([FromBody] TradeDto TradeDto)
        {
            await _TradeService.UpdateTradeWithTradeDto(TradeDto);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
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
                await _TradeRepository.DeleteTradeById(id);
                IEnumerable<TradeDto> listOfTrades = await _TradeService.GetTradeDtoById(id);
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