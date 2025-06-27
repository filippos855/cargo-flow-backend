using cargo_flow_backend.Entities;

namespace cargo_flow_backend.Models.Requests
{
    public class OrderCreateRequest
    {
        public Company Company { get; set; } = null!;
        public Person DeliveryPerson { get; set; } = null!;
        public string Address { get; set; } = string.Empty;
        public DictionaryItem Status { get; set; } = null!;
        public Trip? Trip { get; set; }
    }
}
