using cargo_flow_backend.Data;
using cargo_flow_backend.Entities;
using cargo_flow_backend.Mappings;
using cargo_flow_backend.Models.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cargo_flow_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly CargoFlowDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

        public AuthController(CargoFlowDbContext context)
        {
            _context = context;
        }

        public class LoginRequest
        {
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Users
                .Include(u => u.Role).ThenInclude(d => d.Dictionary)
                .Include(u => u.Person)
                .FirstOrDefaultAsync(u => u.Username == request.Username && u.IsActive);

            if (user == null)
                return Unauthorized("Utilizator inexistent sau inactiv.");

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, request.Password);

            if (result == PasswordVerificationResult.Failed)
                return Unauthorized("Parolă incorectă.");

            return Ok(user.ToDto());
        }
    }
}
