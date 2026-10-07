using System.ComponentModel.DataAnnotations;

namespace MonitoreoAPI.Models
{
    public class Planta
    {
 

        public int Id { get; set; }
        public required string Nombre { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Area>? Areas { get; set; }
    }
}
