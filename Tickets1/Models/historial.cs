using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tickets1.Models
{
    public class historial
    {
        [Key]
        public int id_historial { get; set; }
        public string? mensaje { get; set; }
        public DateTime fecha_cambio { get; set; }

        [ForeignKey("id_ticket")]
        public virtual tickets ticket { get; set; }

        [ForeignKey("id_estadoA")]
        public virtual estado estadoAnterior { get; set; }

        [ForeignKey("id_estadoN")]
        public virtual estado estadoNuevo { get; set; }
    }
}
