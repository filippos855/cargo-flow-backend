namespace cargo_flow_backend.Models.Responses
{
    public class DictionaryItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DictionaryDto Dictionary { get; set; } = null!;
    }
}
