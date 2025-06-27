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
    public class InvoicesController : ControllerBase
    {
        private readonly InvoiceService _invoiceService;
        private readonly DictionaryItemService _dictionaryItemService;
        private readonly CompanyService _companyService;
        private readonly OrderService _orderService;
        private readonly TripService _tripService;

        public InvoicesController(
            InvoiceService invoiceService,
            DictionaryItemService dictionaryItemService,
            CompanyService companyService,
            OrderService orderService,
            TripService tripService)
        {
            _invoiceService = invoiceService;
            _dictionaryItemService = dictionaryItemService;
            _companyService = companyService;
            _orderService = orderService;
            _tripService = tripService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<InvoiceDto>>> GetAll([FromQuery] InvoiceQuery query)
        {
            var result = await _invoiceService.GetPagedAsync(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InvoiceDto>> GetById(int id)
        {
            var invoice = await _invoiceService.GetByIdAsync(id);
            if (invoice == null)
                return NotFound();

            return Ok(invoice);
        }

        [HttpPost]
        public async Task<ActionResult<InvoiceDto>> Create([FromBody] InvoiceCreateRequest request)
        {
            // Validare și lookup entități relaționate
            var invoiceType = await _dictionaryItemService.GetItemByIdAsync(request.InvoiceType.Id);
            if (invoiceType == null) return BadRequest("Tipul facturii invalid.");

            var status = await _dictionaryItemService.GetItemByIdAsync(request.Status.Id);
            if (status == null) return BadRequest("Status invalid.");

            var company = await _companyService.GetEntityByIdAsync(request.Company.Id);
            if (company == null) return BadRequest("Firma nu există.");

            Order? order = null;
            if (request.Order?.Id > 0)
            {
                order = await _orderService.GetEntityByIdAsync(request.Order.Id);
                if (order == null) return BadRequest("Comanda nu există.");
            }

            Trip? trip = null;
            if (request.Trip?.Id > 0)
            {
                trip = await _tripService.GetEntityByIdAsync(request.Trip.Id);
                if (trip == null) return BadRequest("Cursa nu există.");
            }

            var entity = request.ToEntity(invoiceType, status, company, order, trip);
            var created = await _invoiceService.CreateAsync(entity);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created.ToDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] InvoiceUpdateRequest request)
        {
            if (id != request.Id)
                return BadRequest();

            var entity = await _invoiceService.GetEntityByIdAsync(id);
            if (entity == null) return NotFound();

            var invoiceType = await _dictionaryItemService.GetItemByIdAsync(request.InvoiceType.Id);
            if (invoiceType == null) return BadRequest("Tipul facturii invalid.");

            var status = await _dictionaryItemService.GetItemByIdAsync(request.Status.Id);
            if (status == null) return BadRequest("Status invalid.");

            var company = await _companyService.GetEntityByIdAsync(request.Company.Id);
            if (company == null) return BadRequest("Firma nu există.");

            Order? order = null;
            if (request.Order?.Id > 0)
            {
                order = await _orderService.GetEntityByIdAsync(request.Order.Id);
                if (order == null) return BadRequest("Comanda nu există.");
            }

            Trip? trip = null;
            if (request.Trip?.Id > 0)
            {
                trip = await _tripService.GetEntityByIdAsync(request.Trip.Id);
                if (trip == null) return BadRequest("Cursa nu există.");
            }

            request.UpdateEntity(entity, invoiceType, status, company, order, trip);
            await _invoiceService.UpdateAsync(entity);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _invoiceService.DeleteAsync(id);
            return NoContent();
        }
    }
}
