using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rocky.Domain
{
    public class InquiryDetail
    {  
        [Key]
        public int Id { get; set; }

        [Required]
        public int InquiryHeaderId { get; set; }

        [ForeignKey("InquiryHeaderId")]
        public InquiryHeader InquiryHeader { get; set; }

        [Required]
        public int ProductId { get; set; }
        [Required]
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
