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
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<UserDto>>> GetAll([FromQuery] UserQuery query)
        {
            var result = await _userService.GetPagedAsync(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Create([FromBody] UserCreateRequest request)
        {
            try
            {
                var created = await _userService.CreateAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created.ToDto());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserUpdateRequest request)
        {
            if (id != request.Id)
                return BadRequest();

            try
            {
                await _userService.UpdateAsync(request);
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
                await _userService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
