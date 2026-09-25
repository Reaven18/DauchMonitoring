using System.ComponentModel.DataAnnotations;

namespace DauchMonitoring.Models.DTOs.Area
{
    public class AreaDTO
    {
        [Required]
        public string? Nombre { get; set; }
        
        [Range(1, int.MaxValue)]
        public int IdPlanta { get; set; }        
    }
}
