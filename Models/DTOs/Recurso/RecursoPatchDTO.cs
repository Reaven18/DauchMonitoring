using MonitoreoAPI.Models.DTOs.Estado;
using MonitoreoAPI.Models.DTOs.Tipo_recurso;
using System.ComponentModel.DataAnnotations;

namespace MonitoreoAPI.Models.DTOs
{
    public class RecursoPatchDTO
    {
        public int? Id { get; set; }
        public string? Nombre { get; set; }
        
        public int? Id_estado { get; set; }
        public int? Id_tipo_recurso { get; set; }

        public int? Id_area { get; set; }
        public EstadoDTO? Estado { get; set; }
        public TipoRecursoDTO? TipoRecurso { get; set; }
    }
}
