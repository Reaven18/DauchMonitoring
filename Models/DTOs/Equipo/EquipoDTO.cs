using MonitoreoAPI.Models.DTOs.Recurso;

namespace MonitoreoAPI.Models.DTOs.Equipo
{
    public class EquipoDTO : RecursoDTO
    {
        public string? Codigo { get; set; }
        public string? Modelo { get; set; }
        public string? Direccion_IP { get; set; }
        public int? CPU { get; set; }
        public int? Memoria { get; set; }
        public int? Puerto { get; set; }
        public DateTime Heartbeat { get; set; }

    }
}
