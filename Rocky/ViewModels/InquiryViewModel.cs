using Rocky.Domain;
using System.Collections.Generic;

namespace Rocky.UI.Web.ViewModels
{
    public class InquiryViewModel
    {
        public InquiryHeader InquiryHeader { get; set; }

        public IEnumerable<InquiryDetail> InquiryDetail { get; set; }
    }
}