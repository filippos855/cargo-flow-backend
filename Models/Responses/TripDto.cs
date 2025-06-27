namespace cargo_flow_backend.Models.Responses
{
    public class TripDto
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DictionaryItemDto Status { get; set; } = null!;
        public CompanyDto TransportCompany { get; set; } = null!;
        public PersonDto? Driver { get; set; }
        public FleetVehicleDto? TractorUnit { get; set; }
        public FleetVehicleDto? Trailer { get; set; }
        public List<OrderDto>? Orders { get; set; }
    }
}
