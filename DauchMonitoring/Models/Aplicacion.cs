namespace DauchMonitoring.Models
{
    public class Aplicacion : Recurso
    {        
        public string? Codigo { get; set; }
        public string? Version { get; set; }
        public DateTime LastUpdate { get; set; }
        
    }
}
