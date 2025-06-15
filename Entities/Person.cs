namespace cargo_flow_backend.Entities
{
    public class Person
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Cnp { get; set; }
    }
}
