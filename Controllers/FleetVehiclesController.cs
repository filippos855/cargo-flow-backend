using System.ComponentModel.Design;
using cargo_flow_backend.Entities;
using cargo_flow_backend.Mappings;
using cargo_flow_backend.Models.Requests;
using cargo_flow_backend.Models.Responses;
using cargo_flow_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace cargo_flow_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FleetVehiclesController : ControllerBase
    {
        private readonly FleetVehicleService _fleetService;
        private readonly DictionaryItemService _dictionaryItemService;

        public FleetVehiclesController(FleetVehicleService fleetService, DictionaryItemService dictionaryService)
        {
            _fleetService = fleetService;
            _dictionaryItemService = dictionaryService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<FleetVehicleDto>>> GetAll([FromQuery] FleetVehicleQuery query)
        {
            var result = await _fleetService.GetPagedAsync(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FleetVehicleDto>> GetById(int id)
        {
            var vehicle = await _fleetService.GetByIdAsync(id);
            if (vehicle == null)
                return NotFound();

            return Ok(vehicle);
        }

        [HttpPost]
        public async Task<ActionResult<FleetVehicleDto>> Create([FromBody] FleetVehicleCreateRequest request)
        {
            var type = await _dictionaryItemService.GetItemByIdAsync(request.Type.Id);
            if (type == null) return BadRequest("Tipul vehiculului este invalid.");

            var entity = request.ToEntity(type);
            var created = await _fleetService.CreateAsync(entity);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created.ToDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] FleetVehicleUpdateRequest request)
        {
            if (id != request.Id)
                return BadRequest();

            var entity = await _fleetService.GetEntityByIdAsync(id);
            if (entity == null) return NotFound();

            var type = await _dictionaryItemService.GetItemByIdAsync(request.Type.Id);
            if (type == null) return BadRequest("Tipul vehiculului este invalid.");

            request.UpdateEntity(entity, type);
            await _fleetService.UpdateAsync(entity);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _fleetService.DeleteAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

    }
}
