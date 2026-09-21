using static System.Net.Mime.MediaTypeNames;

namespace DauchMonitoring.Models
{
    public class Recurso
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public int IdEstado { get; set; }
        public int IdArea { get; set; }

        public Estado Estado { get; set; }
        public Area Area { get; set; }

        public Equipo Equipo { get; set; }
        public Aplicacion Aplicacion { get; set; }
    }
}
