using System.ComponentModel.DataAnnotations;

namespace cargo_flow_backend.Entities
{
    public class Trip
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Numărul cursei este obligatoriu.")]
        [StringLength(50, ErrorMessage = "Numărul cursei nu poate depăși 50 de caractere.")]
        public string Number { get; set; } = string.Empty;

        [Required(ErrorMessage = "Data de început este obligatorie.")]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "Statusul este obligatoriu.")]
        public DictionaryItem Status { get; set; } = null!;

        [Required(ErrorMessage = "Firma de transport este obligatorie.")]
        public Company TransportCompany { get; set; } = null!;

        public Person? Driver { get; set; }

        public FleetVehicle? TractorUnit { get; set; }

        public FleetVehicle? Trailer { get; set; }

        public ICollection<Order>? Orders { get; set; }

        public ICollection<Invoice>? Invoices { get; set; }
    }
}
