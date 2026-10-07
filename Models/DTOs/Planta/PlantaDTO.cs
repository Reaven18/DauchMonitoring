using System.ComponentModel.DataAnnotations;

namespace MonitoreoAPI.Models.DTOs.Planta
{
    public class PlantaDTO
    {
        [Required]
        public string Nombre { get; set; }
    }
}
