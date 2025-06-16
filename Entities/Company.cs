using System.ComponentModel.DataAnnotations;

namespace cargo_flow_backend.Entities
{
    public class Company
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public string? Cui { get; set; }

        public string? Address { get; set; }

        public Person ContactPerson { get; set; } = null!;

        public ICollection<Order>? Orders { get; set; }
        public ICollection<Trip>? TransportedTrips { get; set; }
        public ICollection<Invoice>? Invoices { get; set; }
    }
}
