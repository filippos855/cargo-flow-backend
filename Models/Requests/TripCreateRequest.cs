using cargo_flow_backend.Entities;

namespace cargo_flow_backend.Models.Requests
{
    public class TripCreateRequest
    {
        public string Number { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DictionaryItem Status { get; set; } = null!;
        public Company TransportCompany { get; set; } = null!;
        public Person? Driver { get; set; }
        public FleetVehicle? TractorUnit { get; set; }
        public FleetVehicle? Trailer { get; set; }
        public List<Order>? Orders { get; set; }
    }
}
