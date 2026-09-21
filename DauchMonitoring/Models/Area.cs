namespace DauchMonitoring.Models
{
    public class Area
    {
        public int Id { get; set; }
        public string Nombre { get; set; }        
        public int IdPlanta { get; set; }      
        public Planta Planta { get; set; }
        public ICollection<Recurso> Recursos { get; set; }
    }
}
