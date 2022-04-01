using Rocky.Domain;
using System.Collections.Generic;

namespace Rocky.ViewModels
{
    public class ProductUserViewModel
    {
        public ProductUserViewModel()
        {
            ProductList = new List<Product>();
        }

        public ApplicationUser ApplicationUser { get; set; }

        public IList<Product> ProductList { get; set; }
    }
}