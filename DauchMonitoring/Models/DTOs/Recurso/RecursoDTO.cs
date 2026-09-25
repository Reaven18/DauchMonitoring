using DauchMonitoring.Models.DTOs.Aplicacion;
using DauchMonitoring.Models.DTOs.Area;
using DauchMonitoring.Models.DTOs.Equipo;
using DauchMonitoring.Models.DTOs.Estado;
using System.ComponentModel.DataAnnotations;

namespace DauchMonitoring.Models.DTOs.Recurso
{
    public class RecursoDTO
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        [RegularExpression("^[a-zA-Z0-9_]+$")]
        public string Nombre { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int IdEstado { get; set; }
        public EstadoDTO Estado { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int IdArea { get; set; }
        
        public AreaResponseDTO Area { get; set; }
        
    }
}
