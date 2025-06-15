using System.ComponentModel.DataAnnotations;

namespace cargo_flow_backend.Entities
{
    public class Invoice
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Numărul facturii este obligatoriu.")]
        [StringLength(50, ErrorMessage = "Numărul facturii nu poate depăși 50 de caractere.")]
        public string Number { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tipul facturii este obligatoriu.")]
        public DictionaryItem InvoiceType { get; set; } = null!;

        [Required(ErrorMessage = "Statusul facturii este obligatoriu.")]
        public DictionaryItem Status { get; set; } = null!;

        [Required(ErrorMessage = "Data emiterii este obligatorie.")]
        public DateTime IssueDate { get; set; }

        [Required(ErrorMessage = "Data scadenței este obligatorie.")]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "Firma este obligatorie.")]
        public Company Company { get; set; } = null!;

        [Required(ErrorMessage = "Suma este obligatorie.")]
        [Range(0, double.MaxValue, ErrorMessage = "Suma trebuie să fie pozitivă.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Moneda este obligatorie.")]
        [StringLength(10, ErrorMessage = "Moneda nu poate depăși 10 caractere.")]
        public string Currency { get; set; } = "RON";

        public Order? Order { get; set; }
        public Trip? Trip { get; set; }
    }
}
