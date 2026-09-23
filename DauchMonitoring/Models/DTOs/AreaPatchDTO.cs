using System.ComponentModel.DataAnnotations;

namespace DauchMonitoring.Models.DTOs
{
    public class AreaPatchDTO
    {        
        public string? Nombre { get; set; }        
        public int? IdPlanta { get; set; }
    }
}
