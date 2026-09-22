namespace DauchMonitoring.Models.DTOs
{
    public class RecursoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public int IdEstado { get; set; }
        public EstadoDTO Estado { get; set; }
        public int IdArea { get; set; }
        
        public AreaDTO Area { get; set; }

        public EquipoDTO Equipo { get; set; }
        public AplicacionDTO Aplicacion { get; set; }
    }
}
