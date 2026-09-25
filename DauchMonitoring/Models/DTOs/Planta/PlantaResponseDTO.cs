using DauchMonitoring.Models.DTOs.Area;

namespace DauchMonitoring.Models.DTOs.Planta
{
    public class PlantaResponseDTO : PlantaDTO
    {
        public int Id { get; set; }
        public List<AreaPatchDTO>? Areas { get; set; }
    }
}
