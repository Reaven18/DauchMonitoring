namespace DauchMonitoring.Models.DTOs
{
    public class PlantaDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public List<AreaDTO> Areas { get; set; }
    }
}
