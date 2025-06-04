using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Tickets1.Models
{
    public class comentarios
    {
        [Key]
        public int id_comentario { get; set; }
        public string? comentario { get; set; }
        public DateTime fecha { get; set; }

        [ForeignKey("id_ticket")]
        public virtual tickets ticket { get; set; }

        [ForeignKey("id_usuarios")]
        public virtual usuarios usuario { get; set; }
    }
}
