using Microsoft.AspNetCore.Mvc.Rendering;
using Rocky.Domain;
using System.Collections.Generic;

namespace Rocky.UI.Web.VIewModels
{
    public class OrderListViewModel
    {
        public IEnumerable<OrderHeader> OrderHList { get; set; }

        public IEnumerable<SelectListItem> StatusList { get; set; }

        public string Status { get; set; }
    }
}