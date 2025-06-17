namespace cargo_flow_backend.Models.Responses
{
    public class FleetVehicleDto
    {
        public int Id { get; set; }
        public string Identifier { get; set; } = string.Empty;
        public DictionaryItemDto Type { get; set; } = null!;
        public DateTime ItpExpiration { get; set; }
        public DateTime RcaExpiration { get; set; }
        public bool IsAvailable { get; set; }
    }
}
