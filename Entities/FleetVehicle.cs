using System.ComponentModel.DataAnnotations;

namespace cargo_flow_backend.Entities
{
    public class FleetVehicle
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Identificatorul vehiculului este obligatoriu.")]
        [StringLength(50, ErrorMessage = "Identificatorul nu poate depăși 50 de caractere.")]
        public string Identifier { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tipul vehiculului este obligatoriu.")]
        public DictionaryItem Type { get; set; } = null!;

        [Required(ErrorMessage = "Data expirării ITP este obligatorie.")]
        public DateTime ItpExpiration { get; set; }

        [Required(ErrorMessage = "Data expirării RCA este obligatorie.")]
        public DateTime RcaExpiration { get; set; }

        public bool IsAvailable { get; set; }


        public ICollection<Trip>? AsTractorUnitInTrips { get; set; }
        public ICollection<Trip>? AsTrailerInTrips { get; set; }
    }
}
