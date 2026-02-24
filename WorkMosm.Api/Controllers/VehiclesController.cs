using Microsoft.AspNetCore.Mvc;
using WorkMosm.Api.Models.Errors;
using WorkMosm.Application.UseCases.GetVehiclesUseCase;

namespace WorkMosm.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ProducesResponseType(typeof(VehicleDtoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public class VehiclesController : ControllerBase
    {
        private readonly IGetVehiclesUseCase _getVehicles;
        private readonly ILogger<VehiclesController> _logger;

        public VehiclesController(IGetVehiclesUseCase getVehicles, ILogger<VehiclesController> logger)
        {
            _getVehicles = getVehicles;
            _logger = logger;
        }

        [HttpGet("showvehicles")]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Starting get all vehicles process");

            var vehicles = await _getVehicles.ExecuteAsync();
            return Ok(vehicles);
        }
    }
}
