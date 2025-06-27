namespace cargo_flow_backend.Models.Requests
{
    public class InvoiceQuery
    {
        public string? Search { get; set; }
        public string Sort { get; set; } = "issueDate";
        public string Direction { get; set; } = "desc";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public DateTime? StartDateFrom { get; set; }
        public DateTime? StartDateTo { get; set; }
    }
}
