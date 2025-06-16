using System.ComponentModel.DataAnnotations;

namespace cargo_flow_backend.Entities
{
    public class DictionaryItem
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int DictionaryId { get; set; }

        public Dictionary Dictionary { get; set; } = null!;
    }
}
