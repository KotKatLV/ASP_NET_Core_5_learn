using Microsoft.AspNetCore.Mvc.Rendering;
using Rocky.Models;
using System.Collections.Generic;

namespace Rocky.VIewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; }

        public IEnumerable<SelectListItem> CategoryDropDownList { get; set; }
    }
}