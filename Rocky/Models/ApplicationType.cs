using System.ComponentModel.DataAnnotations;

namespace Rocky.Models
{
    public class ApplicationType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Application type must be greater than zero")]

        public string Name { get; set; }
    }
}