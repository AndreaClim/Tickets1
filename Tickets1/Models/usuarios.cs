using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tickets1.Models
{
    public enum Autenticacion
    {
        Local,
        Terceros
    }
    public class usuarios
    {
        [Key]
        public int id_usuarios { get; set; }
        public string? nombre { get; set; }
        public string? apellido { get; set; }
        public string? correo { get; set; }
        public string? contrasenia { get; set; }
        public string? telefono { get; set; }
        public int? id_roles { get; set; }

        [Required]
        public Autenticacion autenticacion { get; set; }

        [ForeignKey("id_roles")]
        public virtual roles_t roles { get; set; }
    }
}
