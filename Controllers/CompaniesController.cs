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
    public class CompaniesController : ControllerBase
    {
        private readonly CompanyService _companyService;
        private readonly PersonService _personService;

        public CompaniesController(CompanyService companyService, PersonService personService)
        {
            _companyService = companyService;
            _personService = personService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<CompanyDto>>> GetCompanies([FromQuery] CompanyQuery query)
        {
            var result = await _companyService.GetPagedAsync(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyDto>> GetCompany(int id)
        {
            var company = await _companyService.GetByIdAsync(id);
            if (company == null) return NotFound();
            return Ok(company);
        }

        [HttpPost]
        public async Task<ActionResult<CompanyDto>> CreateCompany([FromBody] CompanyCreateRequest request)
        {
            var person = await _personService.GetEntityByIdAsync(request.ContactPersonId);
            if (person == null) return BadRequest("Persoană de contact invalidă.");

            var company = request.ToEntity(person);
            var created = await _companyService.CreateAsync(company);

            return CreatedAtAction(nameof(GetCompany), new { id = created.Id }, created.ToDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int id, [FromBody] CompanyUpdateRequest request)
        {
            if (id != request.Id)
                return BadRequest();

            var company = await _companyService.GetEntityByIdAsync(id);
            if (company == null) return NotFound();

            var person = await _personService.GetEntityByIdAsync(request.ContactPersonId);
            if (person == null) return BadRequest("Persoană de contact invalidă.");

            request.UpdateEntity(company, person);
            await _companyService.UpdateAsync(company);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var success = await _companyService.DeleteAsync(id);
            if (!success) return NotFound();

            return NoContent();
        }
    }
}
