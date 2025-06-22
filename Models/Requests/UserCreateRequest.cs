using cargo_flow_backend.Entities;

namespace cargo_flow_backend.Models.Requests
{
    public class UserCreateRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DictionaryItem Role { get; set; } = null!;
        public Person Person { get; set; } = null!;
        public bool IsActive { get; set; } = true;
    }
}
