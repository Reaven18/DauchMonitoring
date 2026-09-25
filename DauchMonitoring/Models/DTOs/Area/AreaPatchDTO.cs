using System.ComponentModel.DataAnnotations;

namespace DauchMonitoring.Models.DTOs.Area
{
    public class AreaPatchDTO
    {        
        public int? Id { get; set; }
        public string? Nombre { get; set; }        
        public int? IdPlanta { get; set; }
    }
}
