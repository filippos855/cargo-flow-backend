using cargo_flow_backend.Data;
using cargo_flow_backend.Entities;
using cargo_flow_backend.Mappings;
using cargo_flow_backend.Models.Requests;
using cargo_flow_backend.Models.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace cargo_flow_backend.Services
{
    public class UserService
    {
        private readonly CargoFlowDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

        public UserService(CargoFlowDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<UserDto>> GetPagedAsync(UserQuery query)
        {
            var users = _context.Users
                .Include(u => u.Role).ThenInclude(d => d.Dictionary)
                .Include(u => u.Person)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                string search = query.Search.ToLower();
                users = users.Where(u =>
                    u.Username.ToLower().Contains(search) ||
                    u.Person.FullName.ToLower().Contains(search) ||
                    u.Role.Name.ToLower().Contains(search));
            }

            if (query.IsActive != null)
            {
                users = users.Where(u => u.IsActive == query.IsActive);
            }

            users = query.Sort switch
            {
                "username" => query.Direction == "asc" ? users.OrderBy(u => u.Username) : users.OrderByDescending(u => u.Username),
                "person" => query.Direction == "asc" ? users.OrderBy(u => u.Person.FullName) : users.OrderByDescending(u => u.Person.FullName),
                "role" => query.Direction == "asc" ? users.OrderBy(u => u.Role.Name) : users.OrderByDescending(u => u.Role.Name),
                "isActive" => query.Direction == "asc" ? users.OrderBy(u => u.IsActive) : users.OrderByDescending(u => u.IsActive),
                _ => users.OrderBy(u => u.Username)
            };

            var total = await users.CountAsync();
            var items = await users.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).ToListAsync();

            return new PagedResult<UserDto>
            {
                Items = items.Select(u => u.ToDto()).ToList(),
                TotalCount = total
            };
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _context.Users
                .Include(u => u.Role).ThenInclude(d => d.Dictionary)
                .Include(u => u.Person)
                .FirstOrDefaultAsync(u => u.Id == id);

            return user?.ToDto();
        }

        public async Task<User?> GetEntityByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Role).ThenInclude(d => d.Dictionary)
                .Include(u => u.Person)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> CreateAsync(UserCreateRequest request)
        {
            var role = await _context.DictionaryItems
                .Include(d => d.Dictionary)
                .FirstOrDefaultAsync(r => r.Id == request.Role.Id && r.Dictionary.Name == "Rol utilizator");

            if (role == null) throw new ArgumentException("Rolul este invalid.");

            var person = await _context.Persons.FirstOrDefaultAsync(p => p.Id == request.Person.Id);
            if (person == null) throw new ArgumentException("Persoana este invalidă.");

            var userEntity = new User
            {
                Username = request.Username,
                Role = role,
                Person = person,
                IsActive = request.IsActive
            };

            userEntity.Password = _passwordHasher.HashPassword(userEntity, request.Password);

            _context.Users.Add(userEntity);
            await _context.SaveChangesAsync();

            var createdUser = await _context.Users
                .Include(u => u.Role).ThenInclude(d => d.Dictionary)
                .Include(u => u.Person)
                .FirstOrDefaultAsync(u => u.Id == userEntity.Id);

            return createdUser!;
        }

        public async Task UpdateAsync(UserUpdateRequest request)
        {
            var userEntity = await GetEntityByIdAsync(request.Id);
            if (userEntity == null) throw new KeyNotFoundException("Utilizatorul nu a fost găsit.");

            var role = await _context.DictionaryItems
                .Include(d => d.Dictionary)
                .FirstOrDefaultAsync(r => r.Id == request.Role.Id && r.Dictionary.Name == "Rol utilizator");
            if (role == null) throw new ArgumentException("Rolul este invalid.");

            var person = await _context.Persons.FirstOrDefaultAsync(p => p.Id == request.Person.Id);
            if (person == null) throw new ArgumentException("Persoana este invalidă.");

            userEntity.Username = request.Username;
            userEntity.Role = role;
            userEntity.Person = person;
            userEntity.IsActive = request.IsActive;

            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                userEntity.Password = _passwordHasher.HashPassword(userEntity, request.Password);
            }

            _context.Entry(userEntity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) throw new KeyNotFoundException("Utilizatorul nu a fost găsit.");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
