using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tickets1.Models
{
    [Table("roles_t")]  
    public class roles_t
    {
        [Key]
        public int? id_roles { get; set; }
        public string? nombre { get; set; }
    }
}
