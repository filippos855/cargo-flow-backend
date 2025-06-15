using System.ComponentModel.DataAnnotations;

namespace cargo_flow_backend.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Username-ul este obligatoriu.")]
        [StringLength(50, ErrorMessage = "Username-ul nu poate depăși 50 de caractere.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Parola este obligatorie.")]
        public string PasswordHash { get; set; } = string.Empty;

        [Required(ErrorMessage = "Rolul este obligatoriu.")]
        public DictionaryItem Role { get; set; } = null!;

        [Required(ErrorMessage = "Persoana asociată este obligatorie.")]
        public Person Person { get; set; } = null!;

        public bool IsActive { get; set; } = true;
    }
}
