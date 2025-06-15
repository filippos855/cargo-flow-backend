using System.ComponentModel.DataAnnotations;

namespace cargo_flow_backend.Entities
{
    public class Company
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Numele firmei este obligatoriu.")]
        [StringLength(100, ErrorMessage = "Numele firmei nu poate depăși 100 de caractere.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Codul firmei este obligatoriu.")]
        [StringLength(20, ErrorMessage = "Codul firmei nu poate depăși 20 de caractere.")]
        public string Code { get; set; } = string.Empty;

        [RegularExpression(@"^\d{8,10}$", ErrorMessage = "CUI-ul trebuie să conțină între 8 și 10 cifre.")]
        public string? Cui { get; set; }

        [StringLength(200, ErrorMessage = "Adresa nu poate depăși 200 de caractere.")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Persoana de contact este obligatorie.")]
        public Person ContactPerson { get; set; } = null!;

        public ICollection<Order>? Orders { get; set; }
        public ICollection<Trip>? TransportedTrips { get; set; }
        public ICollection<Invoice>? Invoices { get; set; }
    }
}
