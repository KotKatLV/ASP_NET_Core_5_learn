using Rocky.Domain;
using System.Collections.Generic;

namespace Rocky.UI.Web.ViewModels
{
    public class OrderViewModel
    {
        public OrderHeader OrderHeader { get; set; }

        public IEnumerable<OrderDetail> OrderDetail { get; set; }
    }
}