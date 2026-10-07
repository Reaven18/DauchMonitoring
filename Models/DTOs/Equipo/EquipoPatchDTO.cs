namespace MonitoreoAPI.Models.DTOs.Equipo
{
    public class EquipoPatchDTO 
    {
        public string? Nombre { get; set; }
        public int? Id_estado { get; set; }
        public int? Id_tipo_recurso { get; set; }
        public int? Id_area { get; set; }
        public string? Codigo { get; set; }
        public string? Modelo { get; set; }
        public string? Direccion_IP { get; set; }
        public int? CPU { get; set; }
        public int? Memoria { get; set; }
        public int? Puerto { get; set; }
        public DateTime? Heartbeat { get; set; }
    }
}
