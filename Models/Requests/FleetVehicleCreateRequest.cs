using cargo_flow_backend.Entities;

namespace cargo_flow_backend.Models.Requests
{
    public class FleetVehicleCreateRequest
    {
        public string Identifier { get; set; } = string.Empty;
        public DictionaryItem Type { get; set; } = null!;
        public DateTime ItpExpiration { get; set; }
        public DateTime RcaExpiration { get; set; }
        public bool IsAvailable { get; set; }
    }
}
