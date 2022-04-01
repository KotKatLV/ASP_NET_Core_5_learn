using Rocky.Domain;
using System.Collections.Generic;

namespace Rocky.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Product> Products { get; set; }

        public IEnumerable<Category> Categories { get; set; }
    }
}