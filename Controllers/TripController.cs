using cargo_flow_backend.Entities;
using cargo_flow_backend.Mappings;
using cargo_flow_backend.Models.Requests;
using cargo_flow_backend.Models.Responses;
using cargo_flow_backend.Services;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace cargo_flow_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripsController : ControllerBase
    {
        private readonly TripService _tripService;

        public TripsController(TripService tripService)
        {
            _tripService = tripService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<TripDto>>> GetAll([FromQuery] TripQuery query)
        {
            var result = await _tripService.GetPagedAsync(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TripDto>> GetById(int id)
        {
            var trip = await _tripService.GetByIdAsync(id);
            if (trip == null)
                return NotFound();

            return Ok(trip);
        }

        [HttpPost]
        public async Task<ActionResult<TripDto>> Create([FromBody] TripCreateRequest request)
        {
            if (request.Status == null || request.Status.Id == 0)
                return BadRequest("Statusul este obligatoriu și trebuie să aibă un id valid.");
            if (request.TransportCompany == null || request.TransportCompany.Id == 0)
                return BadRequest("Firma de transport este obligatorie și trebuie să aibă un id valid.");

            try
            {
                var trip = await _tripService.CreateAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = trip.Id }, trip.ToDto());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TripUpdateRequest request)
        {
            if (id != request.Id)
                return BadRequest();

            if (request.Status == null || request.Status.Id == 0)
                return BadRequest("Statusul este obligatoriu și trebuie să aibă un id valid.");
            if (request.TransportCompany == null || request.TransportCompany.Id == 0)
                return BadRequest("Firma de transport este obligatorie și trebuie să aibă un id valid.");

            try
            {
                await _tripService.UpdateAsync(request);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _tripService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("{tripId}/add-order/{orderId}")]
        public async Task<IActionResult> AddOrderToTrip(int tripId, int orderId)
        {
            try
            {
                await _tripService.AddOrderToTripAsync(tripId, orderId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{tripId}/remove-order/{orderId}")]
        public async Task<IActionResult> RemoveOrderFromTrip(int tripId, int orderId)
        {
            try
            {
                await _tripService.RemoveOrderFromTripAsync(tripId, orderId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
