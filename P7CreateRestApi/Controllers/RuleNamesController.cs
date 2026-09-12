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
    public class RuleNamesController : ControllerBase
    {
        private readonly IRuleNameRepository _RuleNameRepository;
        private readonly IRuleNameService _RuleNameService;

        public RuleNamesController(IRuleNameRepository RuleNameRepository, IRuleNameService RuleNameService)
        {
            _RuleNameRepository = RuleNameRepository;
            _RuleNameService = RuleNameService;
        }

        [Authorize(Roles = "Member, Admin")]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllRuleNames()
        {
            IEnumerable<RuleNameDto> listOfRuleNames = await _RuleNameService.GetAllRuleNamesDto();
            if (!listOfRuleNames.Any())
            {
                return NotFound("No information found.");
            }
            return Ok(listOfRuleNames);
        }

        [Authorize(Roles = "Member, Admin")]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetRuleNameById(int id)
        {
            IEnumerable<RuleNameDto> listOfRuleNames = await _RuleNameService.GetRuleNameDtoById(id);

            if (id == 0)
            {
                return BadRequest("Bad request. ID must be an integer and larger than 0.");
            }
            if (!listOfRuleNames.Any())
            {
                return NotFound("The information with the specified ID was not found.");
            }
            return Ok(listOfRuleNames);
        }


        [HttpGet]
        [Route("validate")]
        private IActionResult ValidateRuleNameById([FromBody] RuleNameDto RuleName)
        {
            // TODO: check data valid and save to db, after saving return bid list
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [ServiceFilter(typeof(AsyncActionFilter))]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateRuleName([FromBody] RuleNameDto RuleNameDto)
        {
            await _RuleNameService.CreateRuleNameWithRuleNameDto(RuleNameDto);
            return Ok();
        }


        [Authorize(Roles = "Admin")]
        [ServiceFilter(typeof(AsyncActionFilter))]
        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateRuleNameById([FromBody] RuleNameDto RuleNameDto)
        {
            await _RuleNameService.UpdateRuleNameWithRuleNameDto(RuleNameDto);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteRuleNameById(int id)
        {
            if (id == 0)
            {
                return BadRequest("Bad request. The information ID must be an integer and larger than 0.");
            }
            if (id > 0)
            {
                await _RuleNameRepository.DeleteRuleNameById(id);
                IEnumerable<RuleNameDto> listOfRuleNames = await _RuleNameService.GetRuleNameDtoById(id);
                if (!listOfRuleNames.Any())
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