namespace MonitoreoAPI.Models
{
    public class Tipo_recurso
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<Recurso>? Recursos { get; set; }
    }
}
