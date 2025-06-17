using System.ComponentModel.DataAnnotations;

namespace cargo_flow_backend.Entities
{
    public class FleetVehicle
    {
        public int Id { get; set; }

        public string Identifier { get; set; } = string.Empty;

        public DictionaryItem Type { get; set; } = null!;

        public DateTime ItpExpiration { get; set; }

        public DateTime RcaExpiration { get; set; }

        public bool IsAvailable { get; set; }

        public ICollection<Trip>? AsTractorUnitInTrips { get; set; }
        public ICollection<Trip>? AsTrailerInTrips { get; set; }
    }
}
