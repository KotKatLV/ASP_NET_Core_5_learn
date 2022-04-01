using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Rocky.Domain
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [DisplayName("Display order")]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage ="Display order for category must be greater than zero")]
        public int DisplayOrder { get; set; }
    }
}