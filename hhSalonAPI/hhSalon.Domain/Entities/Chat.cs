using hhSalon.Domain.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hhSalon.Domain.Entities
{
    public class Chat : EntityBase
    {
        [Column("from_id")]
        public string FromId { get; set; }
        [ForeignKey(nameof(FromId))]
        public User FromUser { get; set; }

        [Column("to_id")]
        public string ToId { get; set; }
        [ForeignKey(nameof(ToId))]
        public User ToUser { get; set; }

        [Column("content")]
        public string Content { get; set; }

        [Column("date")]
        public DateTime? Date { get; set; }

        [Column("is_read")]
        public bool IsRead { get; set; }
    }
}
