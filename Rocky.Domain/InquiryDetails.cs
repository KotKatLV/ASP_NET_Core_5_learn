﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocky.Domain
{
    public class InquiryDetails
    {  
        [Key]
        public int Id { get; set; }

        [Required]
        public string InquiryHeaderId { get; set; }

        [ForeignKey("InquiryHeaderId")]
        public InquiryHeader InquiryHeader { get; set; }

        [Required]
        public string ProductId { get; set; }
        [Required]
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}