using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Filters;
using P7CreateRestApi.IRepositories;
using P7CreateRestApi.DTO;
using P7CreateRestApi.IServices;

namespace P7CreateRestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CurvePointsController : ControllerBase
    {
        private readonly ICurvePointRepository _CurvePointRepository;
        private readonly ICurvePointService _CurvePointService;

        public CurvePointsController(ICurvePointRepository curvePointRepository, ICurvePointService curvePointService)
        {
            _CurvePointRepository = curvePointRepository;
            _CurvePointService = curvePointService;
        }

        [Authorize(Roles = "Member, Admin")]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllCurvePoints()
        {
            IEnumerable<CurvePointDto> listOfCurvePoints = await _CurvePointService.GetAllCurvePointsDto();
            if (!listOfCurvePoints.Any())
            {
                return NotFound("No information found.");
            }
            return Ok(listOfCurvePoints);
        }

        [Authorize(Roles = "Member, Admin")]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetCurvePointById(int id)
        {
            IEnumerable<CurvePointDto> listOfCurvePoints = await _CurvePointService.GetCurvePointDtoById(id);

            if (id == 0)
            {
                return BadRequest("Bad request. ID must be an integer and larger than 0.");
            }
            if (!listOfCurvePoints.Any())
            {
                return NotFound("The information with the specified ID was not found.");
            }
            return Ok(listOfCurvePoints);
        }


        [HttpGet]
        [Route("validate")]
        private IActionResult ValidateCurvePointById([FromBody] CurvePointDto curvePoint)
        {
            // TODO: check data valid and save to db, after saving return bid list
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [ServiceFilter(typeof(AsyncActionFilter))]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateCurvePoint([FromBody] CurvePointDto curvePointDto)
        {
            await _CurvePointService.CreateCurvePointWithCurvePointDto(curvePointDto);
            return Ok();
        }


        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateCurvePointById([FromBody] CurvePointDto curvePointDto)
        {
            await _CurvePointService.UpdateCurvePointWithCurvePointDto(curvePointDto);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteCurvePointById(int id)
        {
            if (id == 0)
            {
                return BadRequest("Bad request. The information ID must be an integer and larger than 0.");
            }
            if (id > 0)
            {
                await _CurvePointRepository.DeleteCurvePointById(id);
                IEnumerable<CurvePointDto> listOfCurvePoints = await _CurvePointService.GetCurvePointDtoById(id);
                if (!listOfCurvePoints.Any())
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