//using P7CreateRestApi.Controllers.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Controllers;
using P7CreateRestApi.Domain;
using P7CreateRestApi.IRepositories;
using P7CreateRestApi.Repositories;

namespace P7CreateRestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RuleNamesController : ControllerBase
    {
        private readonly IRuleNameRepository _ruleNameRepository;

        public RuleNamesController(IRuleNameRepository ruleNameRepository)
        {
            _ruleNameRepository = ruleNameRepository;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllRuleNames()
        {
            IEnumerable<RuleName> listOfRuleNames = await _ruleNameRepository.GetAllRuleNames();
            if (!listOfRuleNames.Any())
            {
                return NotFound("No information found.");
            }
            return Ok(listOfRuleNames);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetRuleNameById(int id)
        {
            IEnumerable<RuleName> listOfRuleNames = await _ruleNameRepository.GetRuleNameById(id);

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
        private IActionResult ValidateRuleNameById([FromBody] RuleName ruleName)
        {
            // TODO: check data valid and save to db, after saving return bid list
            return Ok();
        }


        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateRuleName([FromBody] RuleName ruleName)
        {
            // TODO: check required fields, if valid call service to update Bid and return list Bid
            await _ruleNameRepository.CreateRuleName(ruleName);
            return Ok();
        }



        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateRuleNameById([FromBody] RuleName ruleName)
        {
            // TODO: check required fields, if valid call service to update Bid and return list Bid
            await _ruleNameRepository.UpdateRuleName(ruleName);
            return Ok();
        }

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
                await _ruleNameRepository.DeleteRuleNameById(id);
                IEnumerable<RuleName> listOfRuleNames = await _ruleNameRepository.GetRuleNameById(id);
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