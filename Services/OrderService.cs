using cargo_flow_backend.Data;
using cargo_flow_backend.Entities;
using cargo_flow_backend.Mappings;
using cargo_flow_backend.Models.Requests;
using cargo_flow_backend.Models.Responses;
using Microsoft.EntityFrameworkCore;

public class OrderService
{
    private readonly CargoFlowDbContext _context;

    public OrderService(CargoFlowDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<OrderDto>> GetPagedAsync(OrderQuery query)
    {
        var orders = _context.Orders
            .Include(o => o.Company).ThenInclude(c => c.ContactPerson)
            .Include(o => o.DeliveryPerson)
            .Include(o => o.Status).ThenInclude(s => s.Dictionary)
            .Include(o => o.Trip).ThenInclude(t => t.TransportCompany)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.ToLower();
            orders = orders.Where(o =>
                o.Number.ToLower().Contains(search) ||
                o.Company.Name.ToLower().Contains(search) ||
                o.DeliveryPerson.FullName.ToLower().Contains(search) ||
                o.Status.Name.ToLower().Contains(search) ||
                o.Address.ToLower().Contains(search) ||
                (o.Trip != null && o.Trip.Number.ToLower().Contains(search))
            );
        }

        if (query.StartDate.HasValue)
            orders = orders.Where(o => o.CreatedDate >= query.StartDate.Value);

        if (query.EndDate.HasValue)
            orders = orders.Where(o => o.CreatedDate <= query.EndDate.Value);

        orders = query.Sort switch
        {
            "number" => query.Direction == "asc" ? orders.OrderBy(o => o.Number) : orders.OrderByDescending(o => o.Number),
            "company" => query.Direction == "asc" ? orders.OrderBy(o => o.Company.Name) : orders.OrderByDescending(o => o.Company.Name),
            "deliveryPerson" => query.Direction == "asc" ? orders.OrderBy(o => o.DeliveryPerson.FullName) : orders.OrderByDescending(o => o.DeliveryPerson.FullName),
            "status" => query.Direction == "asc" ? orders.OrderBy(o => o.Status.Name) : orders.OrderByDescending(o => o.Status.Name),
            "createdDate" => query.Direction == "asc" ? orders.OrderBy(o => o.CreatedDate) : orders.OrderByDescending(o => o.CreatedDate),
            _ => orders.OrderByDescending(o => o.CreatedDate)
        };

        var total = await orders.CountAsync();
        var items = await orders.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).ToListAsync();

        return new PagedResult<OrderDto>
        {
            Items = items.Select(o => o.ToDto()).ToList(),
            TotalCount = total
        };
    }

    public async Task<OrderDto?> GetByIdAsync(int id)
    {
        var order = await _context.Orders
            .Include(o => o.Company).ThenInclude(c => c.ContactPerson)
            .Include(o => o.DeliveryPerson)
            .Include(o => o.Status).ThenInclude(s => s.Dictionary)
            .Include(o => o.Trip).ThenInclude(t => t.TransportCompany)
            .FirstOrDefaultAsync(o => o.Id == id);

        return order?.ToDto();
    }

    public async Task<Order?> GetEntityByIdAsync(int id)
    {
        return await _context.Orders
            .Include(o => o.Company).ThenInclude(c => c.ContactPerson)
            .Include(o => o.DeliveryPerson)
            .Include(o => o.Status).ThenInclude(s => s.Dictionary)
            .Include(o => o.Trip)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<Order> CreateAsync(OrderCreateRequest request)
    {
        var company = await _context.Companies.FindAsync(request.Company.Id) ?? throw new ArgumentException("Firma inexistentă.");
        var person = await _context.Persons.FindAsync(request.DeliveryPerson.Id) ?? throw new ArgumentException("Persoană inexistentă.");
        var status = await _context.DictionaryItems
            .Include(x => x.Dictionary)
            .FirstOrDefaultAsync(x => x.Id == request.Status.Id)
            ?? throw new ArgumentException("Status invalid.");
        Trip? trip = null;
        if (request.Trip != null)
            trip = await _context.Trips.FindAsync(request.Trip.Id);

        var order = request.ToEntity(company, person, status, trip);

        var prefix = company.Code ?? "ORD";
        var count = await _context.Orders.CountAsync(o => o.Company.Id == company.Id) + 1;
        order.Number = $"{prefix}{count}";

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task UpdateAsync(OrderUpdateRequest request)
    {
        var entity = await GetEntityByIdAsync(request.Id) ?? throw new KeyNotFoundException("Comanda nu există.");
        var company = await _context.Companies.FindAsync(request.Company.Id) ?? throw new ArgumentException("Firma inexistentă.");
        var person = await _context.Persons.FindAsync(request.DeliveryPerson.Id) ?? throw new ArgumentException("Persoană inexistentă.");
        var status = await _context.DictionaryItems
            .Include(x => x.Dictionary)
            .FirstOrDefaultAsync(x => x.Id == request.Status.Id)
            ?? throw new ArgumentException("Status invalid.");
        Trip? trip = null;
        if (request.Trip != null)
            trip = await _context.Trips.FindAsync(request.Trip.Id);

        request.UpdateEntity(entity, company, person, status, trip);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Orders.FindAsync(id) ?? throw new KeyNotFoundException("Comanda nu există.");
        _context.Orders.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<int> CountAllAsync()
    {
        return await _context.Orders.CountAsync();
    }

    public async Task<int> CountPendingAsync()
    {
        return await _context.Orders
            .Include(o => o.Status)
            .Where(o => o.Status.Name == "Creata" || o.Status.Name == "Preluata")
            .CountAsync();
    }
}
