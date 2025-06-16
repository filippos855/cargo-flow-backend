using System.ComponentModel.DataAnnotations;

namespace cargo_flow_backend.Entities
{
    public class Invoice
    {
        public int Id { get; set; }

        public string Number { get; set; } = string.Empty;

        public DictionaryItem InvoiceType { get; set; } = null!;

        public DictionaryItem Status { get; set; } = null!;

        public DateTime IssueDate { get; set; }

        public DateTime DueDate { get; set; }

        public Company Company { get; set; } = null!;

        public decimal Amount { get; set; }

        public string Currency { get; set; } = "RON";

        public Order? Order { get; set; }
        public Trip? Trip { get; set; }
    }
}
