using System.ComponentModel.DataAnnotations;

namespace cargo_flow_backend.Entities
{
    public class Dictionary
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public ICollection<DictionaryItem> Items { get; set; } = new List<DictionaryItem>();
    }
}
