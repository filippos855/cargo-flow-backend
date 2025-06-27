namespace cargo_flow_backend.Models.Responses
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public CompanyDto Company { get; set; } = null!;
        public PersonDto DeliveryPerson { get; set; } = null!;
        public string Address { get; set; } = string.Empty;
        public DictionaryItemDto Status { get; set; } = null!;
        public int? TripId { get; set; }
        public string? TripNumber { get; set; }
    }
}