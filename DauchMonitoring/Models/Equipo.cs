namespace DauchMonitoring.Models
{
    public class Equipo
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Modelo { get; set; }
        public string Ip { get; set; }
        public string Cpu { get; set; }
        public string Memoria { get; set; }
        public int Puerto { get; set; }
        public DateTime Heartbeat { get; set; }

        public Recurso? Recurso { get; set; }
    }

}
