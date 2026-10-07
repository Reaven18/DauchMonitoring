using System.ComponentModel.DataAnnotations;

namespace MonitoreoAPI.Models.DTOs.Area
{
    public class AreaDTO
    {         
        [Required]
        public string Nombre { get; set; }
        [Range(1, int.MaxValue)]
        public int Id_planta { get; set; }
    }
}
