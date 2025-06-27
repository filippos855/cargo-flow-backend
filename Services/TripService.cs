using cargo_flow_backend.Data;
using cargo_flow_backend.Entities;
using cargo_flow_backend.Mappings;
using cargo_flow_backend.Models.Requests;
using cargo_flow_backend.Models.Responses;
using Microsoft.EntityFrameworkCore;

public class TripService
{
    private readonly CargoFlowDbContext _context;

    public TripService(CargoFlowDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<TripDto>> GetPagedAsync(TripQuery query)
    {
        var trips = _context.Trips
            .Include(t => t.TransportCompany).ThenInclude(c => c.ContactPerson)
            .Include(t => t.Status).ThenInclude(s => s.Dictionary)
            .Include(t => t.Driver)
            .Include(t => t.TractorUnit).ThenInclude(f => f.Type).ThenInclude(d => d.Dictionary)
            .Include(t => t.Trailer).ThenInclude(f => f.Type).ThenInclude(d => d.Dictionary)
            .Include(t => t.Orders).ThenInclude(o => o.Company)
            .Include(t => t.Orders).ThenInclude(o => o.Status).ThenInclude(s => s.Dictionary)
            .Include(t => t.Orders).ThenInclude(o => o.DeliveryPerson)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.ToLower();
            trips = trips.Where(t =>
                t.Number.ToLower().Contains(search) ||
                t.TransportCompany.Name.ToLower().Contains(search) ||
                (t.Driver != null && t.Driver.FullName.ToLower().Contains(search)) ||
                t.Status.Name.ToLower().Contains(search)
            );
        }

        if (query.StartDateFrom.HasValue)
            trips = trips.Where(t => t.StartDate >= query.StartDateFrom.Value);

        if (query.StartDateTo.HasValue)
            trips = trips.Where(t => t.StartDate <= query.StartDateTo.Value);

        trips = query.Sort switch
        {
            "number" => query.Direction == "asc" ? trips.OrderBy(t => t.Number) : trips.OrderByDescending(t => t.Number),
            "startDate" => query.Direction == "asc" ? trips.OrderBy(t => t.StartDate) : trips.OrderByDescending(t => t.StartDate),
            "driver" => query.Direction == "asc"
                ? trips.OrderBy(t => t.Driver != null ? t.Driver.FullName : "")
                : trips.OrderByDescending(t => t.Driver != null ? t.Driver.FullName : ""),
            _ => trips.OrderByDescending(t => t.StartDate)
        };

        var total = await trips.CountAsync();
        var items = await trips.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).ToListAsync();

        return new PagedResult<TripDto>
        {
            Items = items.Select(t => t.ToDto()).ToList(),
            TotalCount = total
        };
    }

    public async Task<TripDto?> GetByIdAsync(int id)
    {
        var trip = await _context.Trips
            .Include(t => t.TransportCompany).ThenInclude(c => c.ContactPerson)
            .Include(t => t.Status).ThenInclude(s => s.Dictionary)
            .Include(t => t.Driver)
            .Include(t => t.TractorUnit).ThenInclude(f => f.Type).ThenInclude(d => d.Dictionary)
            .Include(t => t.Trailer).ThenInclude(f => f.Type).ThenInclude(d => d.Dictionary)
            .Include(t => t.Orders).ThenInclude(o => o.Company)
            .Include(t => t.Orders).ThenInclude(o => o.Status).ThenInclude(s => s.Dictionary)
            .Include(t => t.Orders).ThenInclude(o => o.DeliveryPerson)
            .FirstOrDefaultAsync(t => t.Id == id);

        return trip?.ToDto();
    }

    public async Task<Trip?> GetEntityByIdAsync(int id)
    {
        return await _context.Trips
            .Include(t => t.TransportCompany).ThenInclude(c => c.ContactPerson)
            .Include(t => t.Status).ThenInclude(s => s.Dictionary)
            .Include(t => t.Driver)
            .Include(t => t.TractorUnit).ThenInclude(f => f.Type).ThenInclude(d => d.Dictionary)
            .Include(t => t.Trailer).ThenInclude(f => f.Type).ThenInclude(d => d.Dictionary)
            .Include(t => t.Orders).ThenInclude(o => o.Company)
            .Include(t => t.Orders).ThenInclude(o => o.Status).ThenInclude(s => s.Dictionary)
            .Include(t => t.Orders).ThenInclude(o => o.DeliveryPerson)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Trip> CreateAsync(TripCreateRequest request)
    {
        var status = await _context.DictionaryItems
            .Include(x => x.Dictionary)
            .FirstOrDefaultAsync(x => x.Id == request.Status.Id)
            ?? throw new ArgumentException("Status invalid.");

        var company = await _context.Companies
            .Include(c => c.ContactPerson)
            .FirstOrDefaultAsync(c => c.Id == request.TransportCompany.Id)
            ?? throw new ArgumentException("Firma transportatoare inexistentă.");

        Person? driver = null;
        if (request.Driver != null)
            driver = await _context.Persons.FindAsync(request.Driver.Id)
                ?? throw new ArgumentException("Șofer inexistent.");

        FleetVehicle? tractor = null;
        if (request.TractorUnit != null)
            tractor = await _context.FleetVehicles.Include(f => f.Type).ThenInclude(d => d.Dictionary)
                .FirstOrDefaultAsync(f => f.Id == request.TractorUnit.Id)
                ?? throw new ArgumentException("Tractor inexistent.");

        FleetVehicle? trailer = null;
        if (request.Trailer != null)
            trailer = await _context.FleetVehicles.Include(f => f.Type).ThenInclude(d => d.Dictionary)
                .FirstOrDefaultAsync(f => f.Id == request.Trailer.Id)
                ?? throw new ArgumentException("Remorcă inexistentă.");

        List<Order>? orders = null;
        if (request.Orders != null && request.Orders.Count > 0)
        {
            var orderIds = request.Orders.Select(o => o.Id).ToList();
            orders = await _context.Orders
                .Include(o => o.Company)
                .Include(o => o.Status).ThenInclude(s => s.Dictionary)
                .Include(o => o.DeliveryPerson)
                .Where(o => orderIds.Contains(o.Id)).ToListAsync();

            if (orders.Count != orderIds.Count)
                throw new ArgumentException("Cel puțin o comandă nu există.");
        }

        var trip = request.ToEntity(status, company, driver, tractor, trailer, orders);

        _context.Trips.Add(trip);
        await _context.SaveChangesAsync();
        return trip;
    }

    public async Task UpdateAsync(TripUpdateRequest request)
    {
        var entity = await GetEntityByIdAsync(request.Id)
            ?? throw new KeyNotFoundException("Cursa nu există.");

        var status = await _context.DictionaryItems
            .Include(x => x.Dictionary)
            .FirstOrDefaultAsync(x => x.Id == request.Status.Id)
            ?? throw new ArgumentException("Status invalid.");

        var company = await _context.Companies
            .Include(c => c.ContactPerson)
            .FirstOrDefaultAsync(c => c.Id == request.TransportCompany.Id)
            ?? throw new ArgumentException("Firma transportatoare inexistentă.");

        Person? driver = null;
        if (request.Driver != null)
            driver = await _context.Persons.FindAsync(request.Driver.Id)
                ?? throw new ArgumentException("Șofer inexistent.");

        FleetVehicle? tractor = null;
        if (request.TractorUnit != null)
            tractor = await _context.FleetVehicles.Include(f => f.Type).ThenInclude(d => d.Dictionary)
                .FirstOrDefaultAsync(f => f.Id == request.TractorUnit.Id)
                ?? throw new ArgumentException("Tractor inexistent.");

        FleetVehicle? trailer = null;
        if (request.Trailer != null)
            trailer = await _context.FleetVehicles.Include(f => f.Type).ThenInclude(d => d.Dictionary)
                .FirstOrDefaultAsync(f => f.Id == request.Trailer.Id)
                ?? throw new ArgumentException("Remorcă inexistentă.");

        List<Order>? orders = null;
        if (request.Orders != null && request.Orders.Count > 0)
        {
            var orderIds = request.Orders.Select(o => o.Id).ToList();
            orders = await _context.Orders
                .Include(o => o.Company)
                .Include(o => o.Status).ThenInclude(s => s.Dictionary)
                .Include(o => o.DeliveryPerson)
                .Where(o => orderIds.Contains(o.Id)).ToListAsync();

            if (orders.Count != orderIds.Count)
                throw new ArgumentException("Cel puțin o comandă nu există.");
        }

        request.UpdateEntity(entity, status, company, driver, tractor, trailer, orders);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var trip = await _context.Trips.FindAsync(id)
            ?? throw new KeyNotFoundException("Cursa nu există.");

        _context.Trips.Remove(trip);
        await _context.SaveChangesAsync();
    }

    public async Task AddOrderToTripAsync(int tripId, int orderId)
    {
        var trip = await GetEntityByIdAsync(tripId) ?? throw new KeyNotFoundException("Cursa nu există.");
        var order = await _context.Orders.FindAsync(orderId) ?? throw new ArgumentException("Comanda nu există.");

        if (trip.Orders.Any(o => o.Id == orderId))
            throw new InvalidOperationException("Comanda este deja inclusă în această cursă.");

        trip.Orders.Add(order);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveOrderFromTripAsync(int tripId, int orderId)
    {
        var trip = await GetEntityByIdAsync(tripId) ?? throw new KeyNotFoundException("Cursa nu există.");

        var order = trip.Orders.FirstOrDefault(o => o.Id == orderId);
        if (order == null)
            throw new ArgumentException("Comanda nu este inclusă în această cursă.");

        trip.Orders.Remove(order);
        await _context.SaveChangesAsync();
    }

    public async Task<int> CountActiveAsync()
    {
        return await _context.Trips
            .Include(t => t.Status)
            .Where(t => t.Status.Name == "In desfasurare")
            .CountAsync();
    }
}
