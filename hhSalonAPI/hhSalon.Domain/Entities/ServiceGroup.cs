using hhSalon.Domain.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace hhSalon.Domain.Entities
{
    public class ServiceGroup : EntityBase
    {
        [Column("service_id")]
        public int ServiceId { get; set; }
        public Service Service { get; set; }

        [Column("group_id")]
        public int GroupId { get; set; }
        public GroupOfServices Group { get; set; }
    }
}
