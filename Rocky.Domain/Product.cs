using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rocky.Domain
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string ShortDescription { get; set; }

        public string Description { get; set; }

        [Range(1, double.MaxValue)]
        public decimal Price { get; set; }

        public string Image { get; set; }

        [Display(Name = "Category type")]
        public  int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [Display(Name = "Application type")]
        public int ApplicationTypeId { get; set; }

        [ForeignKey("ApplicationTypeId")]
        public virtual ApplicationType ApplicationType { get; set; }

        [NotMapped]
        [Range(1, 1000, ErrorMessage = "Sqft must be greater than 0")]
        public int TempSqFt { get; set; } = 1;
    }
}