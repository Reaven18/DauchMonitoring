namespace DauchMonitoring.Models.DTOs
{
    public class AreaDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int IdPlanta { get; set; }
        public PlantaDTO Planta { get; set; }
        public List<RecursoDTO> Recursos { get; set; }
    }
}
