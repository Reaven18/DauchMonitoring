using System.ComponentModel.DataAnnotations;

namespace MonitoreoAPI.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public required string Correo { get; set; }
        [Required]
        public required string PasswordHash { get; set; }
    }
}
