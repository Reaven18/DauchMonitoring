using MonitoreoAPI.Models.DTOs.Area;
using System.ComponentModel.DataAnnotations;

namespace MonitoreoAPI.Models.DTOs.Planta
{
    public class PlantaResponseDTO : PlantaDTO
    {
        [Required]
        public int Id { get; set; }
        public List<AreaPatchDTO>? Areas { get; set; }
    }
}
