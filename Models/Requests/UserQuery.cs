namespace cargo_flow_backend.Models.Requests
{
    public class UserQuery
    {
        public string? Search { get; set; }
        public string Sort { get; set; } = "username";
        public string Direction { get; set; } = "asc";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public bool? IsActive { get; set; }
    }
}