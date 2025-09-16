using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PruebaTecnica.Modelo
{
    public class Patient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PatientId { get; set; }   // PK, Identity

        [Required]
        [StringLength(10)]
        public string DocumentType { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string DocumentNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(80)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(80)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public DateTime BirthDate { get; set; }

        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [StringLength(120)]
        public string? Email { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
