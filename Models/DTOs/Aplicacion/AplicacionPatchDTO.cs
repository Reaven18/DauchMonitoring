namespace MonitoreoAPI.Models.DTOs.Aplicacion
{
    public class AplicacionPatchDTO
    {
        public string? Nombre { get; set; }
        public int? Id_estado { get; set; }
        public int? Id_area { get; set; }
        public int? Id_tipo_recurso { get; set; }
        public string? Codigo { get; set; }
        public DateTime Last_update { get; set; }
    }
}
