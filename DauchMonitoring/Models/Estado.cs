namespace DauchMonitoring.Models
{
    public class Estado
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }        
        public ICollection<Recurso> Recursos { get; set; }
    }
}
