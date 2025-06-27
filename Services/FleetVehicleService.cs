using cargo_flow_backend.Data;
using cargo_flow_backend.Entities;
using cargo_flow_backend.Mappings;
using cargo_flow_backend.Models.Requests;
using cargo_flow_backend.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace cargo_flow_backend.Services
{
    public class FleetVehicleService
    {
        private readonly CargoFlowDbContext _context;

        public FleetVehicleService(CargoFlowDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<FleetVehicleDto>> GetPagedAsync(FleetVehicleQuery query)
        {
            var fleet = _context.FleetVehicles
                .Include(f => f.Type)
                .ThenInclude(t => t.Dictionary)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                string search = query.Search.ToLower();
                fleet = fleet.Where(f =>
                    f.Identifier.ToLower().Contains(search) ||
                    f.Type.Name.ToLower().Contains(search));
            }

            if (query.ItpExpired == true)
            {
                fleet = fleet.Where(f => f.ItpExpiration < DateTime.Now);
            }

            if (query.RcaExpired == true)
            {
                fleet = fleet.Where(f => f.RcaExpiration < DateTime.Now);
            }

            if (query.IsAvailable == true)
            {
                fleet = fleet.Where(f => f.IsAvailable);
            }

            fleet = query.Sort switch
            {
                "identifier" => query.Direction == "asc" ? fleet.OrderBy(f => f.Identifier) : fleet.OrderByDescending(f => f.Identifier),
                "type" => query.Direction == "asc" ? fleet.OrderBy(f => f.Type.Name) : fleet.OrderByDescending(f => f.Type.Name),
                "itpExpiration" => query.Direction == "asc" ? fleet.OrderBy(f => f.ItpExpiration) : fleet.OrderByDescending(f => f.ItpExpiration),
                "rcaExpiration" => query.Direction == "asc" ? fleet.OrderBy(f => f.RcaExpiration) : fleet.OrderByDescending(f => f.RcaExpiration),
                "isAvailable" => query.Direction == "asc" ? fleet.OrderBy(f => f.IsAvailable) : fleet.OrderByDescending(f => f.IsAvailable),
                _ => fleet.OrderBy(f => f.Identifier)
            };

            var total = await fleet.CountAsync();
            var items = await fleet.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).ToListAsync();

            return new PagedResult<FleetVehicleDto>
            {
                Items = items.Select(f => f.ToDto()).ToList(),
                TotalCount = total
            };
        }

        public async Task<FleetVehicleDto?> GetByIdAsync(int id)
        {
            var vehicle = await _context.FleetVehicles
                .Include(f => f.Type)
                .ThenInclude(t => t.Dictionary)
                .FirstOrDefaultAsync(f => f.Id == id);

            return vehicle?.ToDto();
        }

        public async Task<FleetVehicle?> GetEntityByIdAsync(int id)
        {
            return await _context.FleetVehicles.Include(f => f.Type).ThenInclude(t => t.Dictionary).FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<FleetVehicle> CreateAsync(FleetVehicle vehicle)
        {
            _context.FleetVehicles.Add(vehicle);
            await _context.SaveChangesAsync();
            return vehicle;
        }

        public async Task UpdateAsync(FleetVehicle vehicle)
        {
            _context.Entry(vehicle).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var vehicle = await _context.FleetVehicles.FindAsync(id) ?? throw new KeyNotFoundException("Vehiculul nu a fost găsit.");

            bool usedAsTractor = vehicle.AsTractorUnitInTrips?.Any() ?? false;
            bool usedAsTrailer = vehicle.AsTrailerInTrips?.Any() ?? false;

            if (usedAsTractor || usedAsTrailer)
                throw new InvalidOperationException("Vehiculul nu poate fi șters – este folosit într-o cursă.");

            _context.FleetVehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task<int> CountItpExpiringInDaysAsync(int days)
        {
            var now = DateTime.Now;
            var until = now.AddDays(days);

            return await _context.FleetVehicles
                .Where(f => f.ItpExpiration >= now && f.ItpExpiration <= until)
                .CountAsync();
        }

    }
}
