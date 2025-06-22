namespace cargo_flow_backend.Models.Responses
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public DictionaryItemDto Role { get; set; } = null!;
        public PersonDto Person { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}