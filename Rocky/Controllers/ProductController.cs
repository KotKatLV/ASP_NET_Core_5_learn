using Microsoft.AspNetCore.Mvc;
using Rocky.Data;
using Rocky.Models;
using System.Collections.Generic;
using System.Linq;

namespace Rocky.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList= _db.Product;

            foreach (var product in productList)
            {
                product.Category = _db.Category.FirstOrDefault(c => c.Id == product.CategoryId);
            }

            return View(productList);
        }

        // GET - UPSERT
        [HttpGet]
        public IActionResult UpSert(int? id)
        {
            Product product = new Product();

            if (id == null)
            {
                return View(product);
            }
            else
            {
                product = _db.Product.Find(id);

                if (product == null)
                {
                    return NotFound();
                }

                return View(product);
            }
        }

        // POST - UPSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpSert(Category category)
        {
            if (ModelState.IsValid)
            {
                _db.Category.Add(category);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET - DELETE
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _db.Category.Find(id);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        // POST - DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var category = _db.Category.Find(id);

            if(category == null)
            {
                return NotFound();
            }

            _db.Category.Remove(category);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}