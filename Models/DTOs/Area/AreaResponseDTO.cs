using MonitoreoAPI.Models.DTOs.Planta;
using MonitoreoAPI.Models.DTOs.Recurso;

namespace MonitoreoAPI.Models.DTOs.Area
{
    public class AreaResponseDTO : AreaDTO
    {
        public int Id { get; set; }
        public PlantaDTO? Planta { get; set; }
        public List<RecursoPatchDTO>? Recursos { get; set; }
    }
}
