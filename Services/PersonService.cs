using cargo_flow_backend.Data;
using cargo_flow_backend.Entities;
using cargo_flow_backend.Models.Requests;
using cargo_flow_backend.Models.Responses;
using Microsoft.EntityFrameworkCore;
using cargo_flow_backend.Mappings;

namespace cargo_flow_backend.Services
{
    public class PersonService
    {
        private readonly CargoFlowDbContext _context;

        public PersonService(CargoFlowDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<PersonDto>> GetPagedAsync(PersonQuery query)
        {
            var persons = _context.Persons.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                string search = query.Search.ToLower();
                persons = persons.Where(p =>
                    p.FullName.ToLower().Contains(search) ||
                    (p.Cnp != null && p.Cnp.ToLower().Contains(search)));
            }

            if (query.Sort == "fullName")
            {
                persons = query.Direction == "asc"
                    ? persons.OrderBy(p => p.FullName)
                    : persons.OrderByDescending(p => p.FullName);
            }
            else if (query.Sort == "cnp")
            {
                persons = query.Direction == "asc"
                    ? persons.OrderBy(p => p.Cnp)
                    : persons.OrderByDescending(p => p.Cnp);
            }

            int total = await persons.CountAsync();

            var items = await persons
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PagedResult<PersonDto>
            {
                Items = items.Select(p => p.ToDto()).ToList(),
                TotalCount = total
            };
        }

        public async Task<PersonDto?> GetByIdAsync(int id)
        {
            var person = await _context.Persons.FindAsync(id);
            return person?.ToDto();
        }

        public async Task<Person> CreateAsync(Person person)
        {
            _context.Persons.Add(person);
            await _context.SaveChangesAsync();
            return person;
        }

        public async Task UpdateAsync(Person person)
        {
            _context.Entry(person).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person == null)
                return false;

            bool usedInOrders = await _context.Orders.AnyAsync(o => o.DeliveryPerson.Id == id);
            bool usedInTrips = await _context.Trips.AnyAsync(t => t.Driver != null && t.Driver.Id == id);
            bool usedInCompanies = await _context.Companies.AnyAsync(c => c.ContactPerson.Id == id);
            bool usedInUsers = await _context.Users.AnyAsync(u => u.Person.Id == id);

            if (usedInOrders || usedInTrips || usedInCompanies || usedInUsers)
            {
                throw new InvalidOperationException("Persoana nu poate fi ștearsă – este utilizată în sistem.");
            }

            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<Person?> GetEntityByIdAsync(int id)
        {
            return await _context.Persons.FindAsync(id);
        }

    }
}
