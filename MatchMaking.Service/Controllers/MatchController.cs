using MatchMaking.Common.Types;
using MatchMaking.Shared.DataContracts;
using Microsoft.AspNetCore.Mvc;

namespace MatchMaking.Service.Controllers
{
    [ApiController]
    [Route("match")]
    public class MatchController(IServiceProvider services, ILogger<MatchController> logger) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post(MatchSearchRequest request)
        {
            var service = services.GetService<BaseService<MatchSearchRequest, MatchSearchResponse>>();
            if (service == null)
            {
                return BadRequest("Service not found");
            }

            var response = await service.Handle(request);
            if(response != null && !response.Success)
            {
                return BadRequest(response.Message);
            }

            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<MatchCompleteResponse>> Get([FromQuery]MatchRetreiveRequest retreiveRequest)
        {
            var service = services.GetService<BaseService<MatchRetreiveRequest, MatchCompleteResponse>>();
            if (service == null)
            {
                return StatusCode(500, $"Internal Server Error: Service Not Found");
            }

            var response = await service.Handle(retreiveRequest);
            if (response == null)
            {
                return StatusCode(500, $"Internal Server Error: Empty");
            }

            if (response != null && !response.Success)
            {
                return BadRequest(response.Message);
            }

            return Ok(response);
        }
    }
}
