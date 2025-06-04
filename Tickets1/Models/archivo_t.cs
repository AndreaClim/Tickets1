using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tickets1.Models
{
    public class archivo_t
    {
        [Key]
        public int id_archivot { get; set; }
        public string? link { get; set; }
        public DateTime fecha { get; set; }

        [ForeignKey("id_ticket")]
        public virtual tickets ticket { get; set; }

    }
}
