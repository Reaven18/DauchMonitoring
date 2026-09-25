using DauchMonitoring.Models.DTOs.Estado;

namespace DauchMonitoring.Models.DTOs.Equipo
{
    public class EquipoPatchDTO
    {        
        public string? Nombre { get; set; }
        public int? IdEstado { get; set; }        
        public int? IdArea { get; set; }
        public string? Codigo { get; set; }
        public string? Modelo { get; set; }
        public string? Ip { get; set; }
        public int? Cpu { get; set; }
        public int? Memoria { get; set; }
        public int? Puerto { get; set; }
        public DateTime? Heartbeat { get; set; }
    }
}
