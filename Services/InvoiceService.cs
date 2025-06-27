using cargo_flow_backend.Data;
using cargo_flow_backend.Entities;
using cargo_flow_backend.Mappings;
using cargo_flow_backend.Models.Requests;
using cargo_flow_backend.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace cargo_flow_backend.Services
{
    public class InvoiceService
    {
        private readonly CargoFlowDbContext _context;

        public InvoiceService(CargoFlowDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<InvoiceDto>> GetPagedAsync(InvoiceQuery query)
        {
            var invoices = _context.Invoices
                .Include(i => i.InvoiceType).ThenInclude(t => t.Dictionary)
                .Include(i => i.Status).ThenInclude(s => s.Dictionary)
                .Include(i => i.Company).ThenInclude(c => c.ContactPerson)
                .Include(i => i.Order)
                .Include(i => i.Trip)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                string search = query.Search.ToLower();
                invoices = invoices.Where(i =>
                    i.Number.ToLower().Contains(search) ||
                    i.Company.Name.ToLower().Contains(search) ||
                    (i.Order != null && i.Order.Number.ToLower().Contains(search)) ||
                    (i.Trip != null && i.Trip.Number.ToLower().Contains(search))
                );
            }

            if (query.StartDateFrom.HasValue)
                invoices = invoices.Where(i => i.IssueDate >= query.StartDateFrom.Value);

            if (query.StartDateTo.HasValue)
                invoices = invoices.Where(i => i.IssueDate <= query.StartDateTo.Value);

            invoices = query.Sort switch
            {
                "number" => query.Direction == "asc" ? invoices.OrderBy(i => i.Number) : invoices.OrderByDescending(i => i.Number),
                "invoiceType" => query.Direction == "asc" ? invoices.OrderBy(i => i.InvoiceType.Name) : invoices.OrderByDescending(i => i.InvoiceType.Name),
                "status" => query.Direction == "asc" ? invoices.OrderBy(i => i.Status.Name) : invoices.OrderByDescending(i => i.Status.Name),
                "order" => query.Direction == "asc" ? invoices.OrderBy(i => i.Order != null ? i.Order.Number : "") : invoices.OrderByDescending(i => i.Order != null ? i.Order.Number : ""),
                "trip" => query.Direction == "asc" ? invoices.OrderBy(i => i.Trip != null ? i.Trip.Number : "") : invoices.OrderByDescending(i => i.Trip != null ? i.Trip.Number : ""),
                "company" => query.Direction == "asc" ? invoices.OrderBy(i => i.Company.Name) : invoices.OrderByDescending(i => i.Company.Name),
                "amount" => query.Direction == "asc" ? invoices.OrderBy(i => i.Amount) : invoices.OrderByDescending(i => i.Amount),
                "currency" => query.Direction == "asc" ? invoices.OrderBy(i => i.Currency) : invoices.OrderByDescending(i => i.Currency),
                "issueDate" => query.Direction == "asc" ? invoices.OrderBy(i => i.IssueDate) : invoices.OrderByDescending(i => i.IssueDate),
                "dueDate" => query.Direction == "asc" ? invoices.OrderBy(i => i.DueDate) : invoices.OrderByDescending(i => i.DueDate),
                _ => invoices.OrderByDescending(i => i.IssueDate)
            };

            var total = await invoices.CountAsync();
            var items = await invoices.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).ToListAsync();

            return new PagedResult<InvoiceDto>
            {
                Items = items.Select(i => i.ToDto()).ToList(),
                TotalCount = total
            };
        }

        public async Task<InvoiceDto?> GetByIdAsync(int id)
        {
            var invoice = await _context.Invoices
                .Include(i => i.InvoiceType).ThenInclude(t => t.Dictionary)
                .Include(i => i.Status).ThenInclude(s => s.Dictionary)
                .Include(i => i.Company).ThenInclude(c => c.ContactPerson)
                .Include(i => i.Order)
                .Include(i => i.Trip)
                .FirstOrDefaultAsync(i => i.Id == id);

            return invoice?.ToDto();
        }

        public async Task<Invoice?> GetEntityByIdAsync(int id)
        {
            return await _context.Invoices
                .Include(i => i.InvoiceType)
                .Include(i => i.Status)
                .Include(i => i.Company).ThenInclude(c => c.ContactPerson)
                .Include(i => i.Order)
                .Include(i => i.Trip)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Invoice> CreateAsync(Invoice invoice)
        {
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();
            return invoice;
        }

        public async Task UpdateAsync(Invoice invoice)
        {
            _context.Entry(invoice).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id) ?? throw new KeyNotFoundException("Factura nu a fost găsită.");
            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();
        }

        public async Task<int> CountUnpaidAsync()
        {
            return await _context.Invoices
                .Include(i => i.Status)
                .Where(i => i.Status.Name == "Neachitata")
                .CountAsync();
        }

    }
}
