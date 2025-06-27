public class OrderQuery
{
    public string? Search { get; set; }
    public string Sort { get; set; } = "createdDate";
    public string Direction { get; set; } = "asc";
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}