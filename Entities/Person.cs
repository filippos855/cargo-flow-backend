using System.ComponentModel.DataAnnotations;

namespace cargo_flow_backend.Entities
{
    public class Person
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Numele complet este obligatoriu.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Numele complet trebuie să aibă între 3 și 100 de caractere.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "CNP-ul este obligatoriu.")]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "CNP-ul trebuie să conțină exact 13 cifre.")]
        public string Cnp { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Numărul de telefon nu este valid.")]
        public string? Phone { get; set; }

        [EmailAddress(ErrorMessage = "Adresa de email nu este validă.")]
        public string? Email { get; set; }

        public ICollection<Order>? DeliveryOrders { get; set; }
        public ICollection<Trip>? DrivenTrips { get; set; }
        public ICollection<Company>? ContactForCompanies { get; set; }
        public ICollection<User>? Users { get; set; }
    }
}
