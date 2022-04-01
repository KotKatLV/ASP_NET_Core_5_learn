using Microsoft.AspNetCore.Mvc.Rendering;
using Rocky.Domain;
using System.Collections.Generic;

namespace Rocky.DAL.Repository.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product obj);

        IEnumerable<SelectListItem> GetAllDropdownList(string obj);
    }
}