using hhSalon.Domain.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hhSalon.Domain.Entities
{
    public class Schedule : EntityBase
    {
        [Column("worker_id")]
        public string WorkerId { get; set; }
        public Worker Worker { get; set; }

        [StringLength(10)]
        [Column("day")]
        public string Day { get; set; }

        [Column("start", TypeName = "time")]
        public TimeSpan Start { get; set; }

        [Column("end", TypeName = "time")]
        public TimeSpan End { get; set; }
    }
}
