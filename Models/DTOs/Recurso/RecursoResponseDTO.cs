using MonitoreoAPI.Models.DTOs.Aplicacion;
using MonitoreoAPI.Models.DTOs.Area;
using MonitoreoAPI.Models.DTOs.Equipo;
using MonitoreoAPI.Models.DTOs.Estado;
using System.ComponentModel.DataAnnotations;

namespace MonitoreoAPI.Models.DTOs.Recurso
{
    public class RecursoResponseDTO : RecursoDTO
    {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }       
    }
}
