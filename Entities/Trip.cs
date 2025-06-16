using System.ComponentModel.DataAnnotations;

namespace cargo_flow_backend.Entities
{
    public class Trip
    {
        public int Id { get; set; }

        public string Number { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DictionaryItem Status { get; set; } = null!;

        public Company TransportCompany { get; set; } = null!;

        public Person? Driver { get; set; }

        public FleetVehicle? TractorUnit { get; set; }

        public FleetVehicle? Trailer { get; set; }

        public ICollection<Order>? Orders { get; set; }

        public ICollection<Invoice>? Invoices { get; set; }
    }
}
