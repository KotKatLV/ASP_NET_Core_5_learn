using Microsoft.AspNetCore.Mvc.Rendering;
using Rocky.Domain;
using System.Collections.Generic;

namespace Rocky.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; }

        public IEnumerable<SelectListItem> CategoryDropDownList { get; set; }

        public IEnumerable<SelectListItem> ApplicationTypeDropDownList { get; set; }
    }
}