using MonitoreoAPI.Models.DTOs.Recurso;

namespace MonitoreoAPI.Models.DTOs.Aplicacion
{
    public class AplicacionDTO : RecursoDTO
    {
        public string? Codigo { get; set; }
        public DateTime Last_update { get; set; }
    }
}
