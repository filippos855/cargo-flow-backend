using System.ComponentModel.DataAnnotations;

namespace cargo_flow_backend.Entities
{
    public class Order
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Numărul comenzii este obligatoriu.")]
        [StringLength(50, ErrorMessage = "Numărul comenzii nu poate depăși 50 de caractere.")]
        public string Number { get; set; } = string.Empty;

        [Required(ErrorMessage = "Data creării este obligatorie.")]
        public DateTime CreatedDate { get; set; }

        [Required(ErrorMessage = "Firma este obligatorie.")]
        public Company Company { get; set; } = null!;

        [Required(ErrorMessage = "Persoana de livrare este obligatorie.")]
        public Person DeliveryPerson { get; set; } = null!;

        [Required(ErrorMessage = "Adresa este obligatorie.")]
        [StringLength(200, ErrorMessage = "Adresa nu poate depăși 200 de caractere.")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Statusul este obligatoriu.")]
        public DictionaryItem Status { get; set; } = null!;

        public Trip? Trip { get; set; }

        public ICollection<Invoice>? Invoices { get; set; }
    }
}
