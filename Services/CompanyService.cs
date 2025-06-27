using cargo_flow_backend.Data;
using cargo_flow_backend.Entities;
using cargo_flow_backend.Models.Requests;
using cargo_flow_backend.Models.Responses;
using cargo_flow_backend.Mappings;
using Microsoft.EntityFrameworkCore;

namespace cargo_flow_backend.Services
{
    public class CompanyService
    {
        private readonly CargoFlowDbContext _context;

        public CompanyService(CargoFlowDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<CompanyDto>> GetPagedAsync(CompanyQuery query)
        {
            var companies = _context.Companies.Include(c => c.ContactPerson).AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                string search = query.Search.ToLower();
                companies = companies.Where(c =>
                    c.Name.ToLower().Contains(search) ||
                    c.Code.ToLower().Contains(search) ||
                    (c.Cui != null && c.Cui.ToLower().Contains(search)));
            }

            companies = query.Sort switch
            {
                "name" => query.Direction == "asc"
                    ? companies.OrderBy(c => c.Name)
                    : companies.OrderByDescending(c => c.Name),

                "code" => query.Direction == "asc"
                    ? companies.OrderBy(c => c.Code)
                    : companies.OrderByDescending(c => c.Code),

                "cui" => query.Direction == "asc"
                    ? companies.OrderBy(c => c.Cui)
                    : companies.OrderByDescending(c => c.Cui),

                "address" => query.Direction == "asc"
                    ? companies.OrderBy(c => c.Address)
                    : companies.OrderByDescending(c => c.Address),

                "contactPerson" => query.Direction == "asc"
                    ? companies.OrderBy(c => c.ContactPerson.FullName)
                    : companies.OrderByDescending(c => c.ContactPerson.FullName),

                _ => companies.OrderBy(c => c.Name) // default
            };


            int total = await companies.CountAsync();
            var items = await companies
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PagedResult<CompanyDto>
            {
                Items = items.Select(c => c.ToDto()).ToList(),
                TotalCount = total
            };
        }

        public async Task<CompanyDto?> GetByIdAsync(int id)
        {
            var company = await _context.Companies
                .Include(c => c.ContactPerson)
                .FirstOrDefaultAsync(c => c.Id == id);

            return company?.ToDto();
        }

        public async Task<Company?> GetEntityByIdAsync(int id)
        {
            return await _context.Companies.Include(c => c.ContactPerson).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Company> CreateAsync(Company company)
        {
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();
            return company;
        }

        public async Task UpdateAsync(Company company)
        {
            _context.Entry(company).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
                return false;

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> CountActiveAsync()
        {
            return await _context.Companies.CountAsync();
        }
    }
}
