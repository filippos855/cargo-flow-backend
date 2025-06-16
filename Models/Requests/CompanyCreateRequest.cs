using cargo_flow_backend.Entities;

namespace cargo_flow_backend.Models.Requests
{
    public class CompanyCreateRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Cui { get; set; }
        public string? Address { get; set; }
        public Person ContactPerson { get; set; } = null!;
    }
}
