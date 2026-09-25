using DauchMonitoring.Models.DTOs.Recurso;
using System.ComponentModel.DataAnnotations;

namespace DauchMonitoring.Models.DTOs.Equipo
{
    public class EquipoDTO : RecursoDTO
    {            
        public string? Codigo { get; set; }
        public string? Modelo { get; set; }
        public string? Ip { get; set; }
        [Range(0, 100)]
        public int? Cpu { get; set; }
        [Range(0, 100)]
        public int? Memoria { get; set; }
        [Range(1, int.MaxValue)]
        public int? Puerto { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime Heartbeat { get; set; }
        
    }
}
