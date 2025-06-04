using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tickets1.Models;

namespace Tickets1.Models
{
    public class tickets
    {
        [Key]
        public int id_ticket { get; set; }
        public string? titulo { get; set; }
        public string? descripcion { get; set; }
        public string? nombre_app { get; set; }
        public DateTime fecha_creacion { get; set; }

        [ForeignKey("id_usuarioC")]
        public virtual usuarios usuarioCliente { get; set; }

        [ForeignKey("id_usuarioE")]
        public virtual usuarios usuarioEmpleado { get; set; }

        [ForeignKey("id_cat")]
        public virtual categoria categorias { get; set; }

        [ForeignKey("id_estado")]
        public virtual estado estados { get; set; }

        [ForeignKey("id_prioridad")]
        public virtual prioridad prioridades { get; set; }

        public virtual ICollection<comentarios> comentarios { get; set; }
        public virtual ICollection<archivo_t> archivost { get; set; }

        public int id_usuarioE { get; set; }
        public int id_cat { get; set; }
        public int id_estado { get; set; }
        public int id_prioridad { get; set; }

    }
}
