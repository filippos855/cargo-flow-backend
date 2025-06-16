using System.ComponentModel.DataAnnotations;

namespace cargo_flow_backend.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public DictionaryItem Role { get; set; } = null!;

        public Person Person { get; set; } = null!;

        public bool IsActive { get; set; } = true;
    }
}
