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
    public class PersonsController : ControllerBase
    {
        private readonly PersonService _personService;

        public PersonsController(PersonService personService)
        {
            _personService = personService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<PersonDto>>> GetPersons([FromQuery] PersonQuery query)
        {
            var result = await _personService.GetPagedAsync(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PersonDto>> GetPerson(int id)
        {
            var person = await _personService.GetByIdAsync(id);
            if (person == null)
                return NotFound();
            return Ok(person);
        }

        [HttpPost]
        public async Task<ActionResult<PersonDto>> CreatePerson([FromBody] PersonCreateRequest request)
        {
            Console.WriteLine($"CNP primit: '{request.Cnp}', lungime: {request.Cnp.Length}");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState invalid:");
                foreach (var kvp in ModelState)
                {
                    Console.WriteLine($" - {kvp.Key}: {string.Join(", ", kvp.Value.Errors.Select(e => e.ErrorMessage))}");
                }
                return BadRequest(ModelState);
            }


            var person = request.ToEntity();
            var created = await _personService.CreateAsync(person);
            return CreatedAtAction(nameof(GetPerson), new { id = created.Id }, created.ToDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePerson(int id, [FromBody] PersonUpdateRequest request)
        {
            if (id != request.Id)
                return BadRequest();

            var person = await _personService.GetEntityByIdAsync(id);
            if (person == null) return NotFound();

            request.UpdateEntity(person);
            await _personService.UpdateAsync(person);

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            try
            {
                var success = await _personService.DeleteAsync(id);
                if (!success)
                    return NotFound();

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
