using hhSalon.Domain.Abstract;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace hhSalon.Domain.Entities
{
    [Table("groups_of_services")]
	public class GroupOfServices:IEntityBase
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [StringLength(45)]
        [Column("name")]
		
		public string Name { get; set; }


		[Column("img_url")]
        public string ImgUrl { get; set; } 


        //[Required(ErrorMessage = "Please choose image")]
        //[NotMapped]
        //public IFormFile? Img { get; set; } 


        
        public List<ServiceGroup> Services_Groups { get; set; }
        public List<WorkerGroup> Workers_Groups { get; set; }
    }
}
