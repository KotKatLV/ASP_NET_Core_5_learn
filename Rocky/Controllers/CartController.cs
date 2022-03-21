using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rocky.Data;
using Rocky.Models;
using Rocky.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Rocky.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CartController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {

            List<ShoppingCart> shoppingCarts = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null 
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                // session exists
                shoppingCarts = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).ToList();
            }

            List<int> prodInCart = shoppingCarts.Select(p => p.ProductId).ToList();
            IEnumerable<Product> productList = _db.Product.Where(p => prodInCart.Contains(p.Id));

            return View(productList);
        }
    }
}
