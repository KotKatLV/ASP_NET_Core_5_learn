using System.ComponentModel.DataAnnotations;

namespace Rocky.Domain
{
    public class ApplicationType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]

        public string Name { get; set; }
    }
}