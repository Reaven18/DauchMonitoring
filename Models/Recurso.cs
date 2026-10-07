using static System.Net.Mime.MediaTypeNames;

namespace MonitoreoAPI.Models
{
    public class Recurso
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public  int Id_estado { get; set; }
        public  int Id_area { get; set; }

        public int Id_tipo_recurso { get; set; }
        public virtual Area? Area { get; set; }
        public virtual Tipo_recurso? TipoRecurso { get; set; }
        public virtual Estado? Estado { get; set; }
    }
}
