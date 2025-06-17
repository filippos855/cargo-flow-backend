using cargo_flow_backend.Mappings;
using cargo_flow_backend.Models.Requests;
using cargo_flow_backend.Models.Responses;
using cargo_flow_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace cargo_flow_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DictionaryItemsController : ControllerBase
    {
        private readonly DictionaryItemService _service;

        public DictionaryItemsController(DictionaryItemService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<DictionaryItemDto>>> GetByDictionaryName([FromQuery] DictionaryItemQuery query)
        {
            if (string.IsNullOrWhiteSpace(query.DictionaryName))
                return BadRequest("Numele dicționarului este obligatoriu.");

            var items = await _service.GetItemsByQueryAsync(query);
            return Ok(items.Select(i => i.ToDto()).ToList());
        }
    }
}
