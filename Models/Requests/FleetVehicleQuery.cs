namespace cargo_flow_backend.Models.Requests
{
    public class FleetVehicleQuery
    {
        public string? Search { get; set; }
        public string Sort { get; set; } = "identifier";
        public string Direction { get; set; } = "asc";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public bool? ItpExpired { get; set; }
        public bool? RcaExpired { get; set; }
        public bool? IsAvailable { get; set; }
    }
}
