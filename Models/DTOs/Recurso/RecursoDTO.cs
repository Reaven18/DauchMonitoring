using MonitoreoAPI.Models.DTOs.Area;
using MonitoreoAPI.Models.DTOs.Estado;
using MonitoreoAPI.Models.DTOs.Tipo_recurso;
using System.ComponentModel.DataAnnotations;

namespace MonitoreoAPI.Models.DTOs.Recurso
{
    public class RecursoDTO
    {
        [Required]
        [StringLength(100)]
        [RegularExpression("^[a-zA-Z0-9_]+$")]
        public string Nombre { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Id_estado { get; set; }
        public EstadoDTO? Estado { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Id_tipo_recurso { get; set; }
        public TipoRecursoDTO? TipoRecurso { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Id_area { get; set; }
        public virtual AreaResponseDTO? Area { get; set; }

    }
}
