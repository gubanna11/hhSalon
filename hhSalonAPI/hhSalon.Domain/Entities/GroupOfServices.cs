using hhSalon.Domain.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace hhSalon.Domain.Entities
{
    [Table("groups_of_services")]
    public class GroupOfServices : EntityBase
    {
        [Required]
        [StringLength(45)]
        [Column("name")]
        public string Name { get; set; }

        [Column("img_url")]
        public string ImgUrl { get; set; }

        public List<ServiceGroup> Services_Groups { get; set; }
        public List<WorkerGroup> Workers_Groups { get; set; }
    }
}
