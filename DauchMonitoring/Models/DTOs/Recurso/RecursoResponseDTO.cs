using DauchMonitoring.Models.DTOs.Aplicacion;
using DauchMonitoring.Models.DTOs.Equipo;

namespace DauchMonitoring.Models.DTOs.Recurso
{
    public class RecursoResponseDTO : RecursoDTO
    {
        public EquipoDTO? Equipo { get; set; }
        public AplicacionDTO? Aplicacion { get; set; }
    }
}
