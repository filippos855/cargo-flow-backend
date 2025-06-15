using System.ComponentModel.DataAnnotations;

namespace cargo_flow_backend.Entities
{
    public class Dictionary
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Numele dicționarului este obligatoriu.")]
        [StringLength(100, ErrorMessage = "Numele dicționarului nu poate depăși 100 de caractere.")]
        public string Name { get; set; } = string.Empty;

        public ICollection<DictionaryItem> Items { get; set; } = new List<DictionaryItem>();
    }
}
