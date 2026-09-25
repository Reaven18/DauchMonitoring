using DauchMonitoring.Models.DTOs.Planta;
using DauchMonitoring.Models.DTOs.Recurso;

namespace DauchMonitoring.Models.DTOs.Area
{
    public class AreaResponseDTO : AreaDTO
    {
        public int Id { get; set; }
        public PlantaDTO? Planta { get; set; }
        public List<RecursoDTO>? Recursos { get; set; }
    }
}
