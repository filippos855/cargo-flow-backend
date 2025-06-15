using System.ComponentModel.DataAnnotations;

namespace cargo_flow_backend.Entities
{
    public class DictionaryItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Numele este obligatoriu.")]
        [StringLength(100, ErrorMessage = "Numele nu poate depăși 100 de caractere.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Id-ul dicționarului este obligatoriu.")]
        public int DictionaryId { get; set; }

        public Dictionary Dictionary { get; set; } = null!;
    }
}
