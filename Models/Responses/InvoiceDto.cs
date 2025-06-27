namespace cargo_flow_backend.Models.Responses
{
    public class InvoiceDto
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public DictionaryItemDto InvoiceType { get; set; } = null!;
        public DictionaryItemDto Status { get; set; } = null!;
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public CompanyDto Company { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "RON";
        public OrderDto? Order { get; set; }
        public TripDto? Trip { get; set; }
    }
}
