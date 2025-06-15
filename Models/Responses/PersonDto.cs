namespace cargo_flow_backend.Models.Responses
{
    public class PersonDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Cnp { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}