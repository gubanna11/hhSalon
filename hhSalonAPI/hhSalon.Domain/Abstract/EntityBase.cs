using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hhSalon.Domain.Abstract
{
    public class EntityBase
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
    }
}
