using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Domain;
using P7CreateRestApi.IRepositories;


namespace P7CreateRestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CurvePointsController : ControllerBase
    {
        
        private readonly ICurvePointRepository _curvePointRepository;

        public CurvePointsController(ICurvePointRepository curvePointRepository)
        {
            _curvePointRepository = curvePointRepository;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllCurvePoints()
        {
            IEnumerable<CurvePoint> listOfCurvePoints = await _curvePointRepository.GetAllCurvePoints();
            if (!listOfCurvePoints.Any())
            {
                return NotFound("No information found.");
            }
            return Ok(listOfCurvePoints);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetCurvePointById(int id)
        {
            IEnumerable<CurvePoint> listOfCurvePoints = await _curvePointRepository.GetCurvePointById(id);

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
        private IActionResult ValidateCurvePointById([FromBody] CurvePoint curvePoint)
        {
            // TODO: check data valid and save to db, after saving return bid list
            return Ok();
        }


        [HttpPost]
        [Route("")]
        public IActionResult CreateCurvePoint([FromBody] CurvePoint curvePoint)
        {
            // TODO: check required fields, if valid call service to update Bid and return list Bid
            _curvePointRepository.CreateCurvePoint(curvePoint);
            return Ok();
        }



        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateCurvePointById([FromBody] CurvePoint curvePoint)
        {
            // TODO: check required fields, if valid call service to update Bid and return list Bid
            await _curvePointRepository.UpdateCurvePoint(curvePoint);
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteCurvePointById(int id)
        {
            if (id == 0)
            {
                return BadRequest("Bad request. User ID must be an integer and larger than 0.");
            }
            if (id > 0)
            {
                _curvePointRepository.DeleteCurvePointById(id);
                IEnumerable<CurvePoint> listOfCurvePoints = await _curvePointRepository.GetCurvePointById(id);
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