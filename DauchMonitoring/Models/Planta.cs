namespace DauchMonitoring.Models
{
    public class Planta
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public ICollection<Area> Areas { get; set; }
    }
}
