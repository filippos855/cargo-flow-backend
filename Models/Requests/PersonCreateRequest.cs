using System.ComponentModel.DataAnnotations;

namespace cargo_flow_backend.Models.Requests
{
    public class PersonCreateRequest
    {
        public string FullName { get; set; } = string.Empty;

        public string Cnp { get; set; } = string.Empty;

        public string? Phone { get; set; }

        public string? Email { get; set; }
    }
}
