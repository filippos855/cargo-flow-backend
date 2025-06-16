using System.ComponentModel.DataAnnotations;

namespace cargo_flow_backend.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public string Number { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }

        public Company Company { get; set; } = null!;

        public Person DeliveryPerson { get; set; } = null!;

        public string Address { get; set; } = string.Empty;

        public DictionaryItem Status { get; set; } = null!;

        public Trip? Trip { get; set; }

        public ICollection<Invoice>? Invoices { get; set; }
    }
}
