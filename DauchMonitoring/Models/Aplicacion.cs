namespace DauchMonitoring.Models
{
    public class Aplicacion
    {
        public int Id { get; set; }
        public string? Codigo { get; set; }
        public string? Version { get; set; }
        public DateTime LastUpdate { get; set; }

        public Recurso? Recurso { get; set; }
    }
}
