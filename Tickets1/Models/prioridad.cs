using System.ComponentModel.DataAnnotations;

namespace Tickets1.Models
{
    public class prioridad
    {
        [Key]
        public int id_prioridad { get; set; }
        public string? nombre { get; set; }
    }
}
