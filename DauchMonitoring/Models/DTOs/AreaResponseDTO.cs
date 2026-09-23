namespace DauchMonitoring.Models.DTOs
{
    public class AreaResponseDTO : AreaDTO
    {
        public int Id { get; set; }
        public PlantaDTO? Planta { get; set; }
        public List<RecursoDTO>? Recursos { get; set; }
    }
}
