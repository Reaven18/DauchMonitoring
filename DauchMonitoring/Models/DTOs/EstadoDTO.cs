namespace DauchMonitoring.Models.DTOs
{
    public class EstadoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public List<RecursoDTO> Recursos { get; set; }
    }
}
