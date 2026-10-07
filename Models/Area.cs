namespace MonitoreoAPI.Models
{
    public class Area
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required int Id_planta { get; set; }
        public virtual Planta? Planta { get; set; }
        public virtual ICollection<Recurso>? Recursos { get; set; }
    }
}
