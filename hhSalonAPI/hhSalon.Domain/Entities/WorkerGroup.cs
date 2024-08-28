using hhSalon.Domain.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace hhSalon.Domain.Entities
{
    public class WorkerGroup : EntityBase
    {
        [Column("worker_id")]
        public string WorkerId { get; set; }
        public Worker Worker { get; set; }

        [Column("group_id")]
        public int GroupId { get; set; }
        public GroupOfServices Group { get; set; }
    }
}
