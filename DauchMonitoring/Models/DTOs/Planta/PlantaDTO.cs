using System.ComponentModel.DataAnnotations;

namespace DauchMonitoring.Models.DTOs.Planta
{
    public class PlantaDTO
    {
        [Required]                
        public string? Nombre { get; set; }        
    }
}
