namespace cargo_flow_backend.Models.Responses
{
    public class CompanyDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Cui { get; set; }
        public string? Address { get; set; }
        public PersonDto ContactPerson { get; set; } = null!;
    }
}
