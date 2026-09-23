namespace DauchMonitoring.Models.DTOs
{
    public class PlantaResponseDTO : PlantaDTO
    {
        public int Id { get; set; }
        public List<AreaDTO>? Areas { get; set; }
    }
}
