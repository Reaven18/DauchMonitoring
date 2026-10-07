namespace MonitoreoAPI.Models
{
    public class Estado
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public string? Descripcion { get; set; }        
        public virtual ICollection<Recurso>? Recursos { get; set; }
    }
}
