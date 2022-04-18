using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Rocky.DAL.Repository.Interfaces;
using Rocky.Domain;
using Rocky.Utils;
using Rocky.ViewModels;
using System;
using System.IO;
using System.Linq;

namespace Rocky.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IProductRepository productRepository, IWebHostEnvironment webHostEnvironment)
        {
            _productRepository = productRepository;
            _webHostEnvironment = webHostEnvironment;   
        }

        public IActionResult Index()
        {
            IQueryable<Product> productList = _productRepository.GetAll(includProps: "Category, ApplicationType");
            return View(productList);
        }

        // GET - UPSERT
        [HttpGet]
        public IActionResult UpSert(int? id)
        {
            ProductViewModel productViewModel = new ProductViewModel()
            {
                Product = new Product(),
                CategoryDropDownList = _productRepository.GetAllDropdownList(WC.CategoryName),
                ApplicationTypeDropDownList = _productRepository.GetAllDropdownList(WC.ApplicationTypeName),
            };

            if (id == null)
            {
                return View(productViewModel);
            }
            else
            {
                productViewModel.Product = _productRepository.GetById(id.GetValueOrDefault());

                if (productViewModel.Product == null)
                {
                    return NotFound();
                }

                return View(productViewModel);
            }
        }

        // POST - UPSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpSert(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (productViewModel.Product.Id == 0)
                {
                    string upload = webRootPath + WC.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productViewModel.Product.Image = fileName + extension;
                    _productRepository.Add(productViewModel.Product);
                }
                else
                {
                    var objFromDb = _productRepository.FirstOrDefault(x => x.Id == productViewModel.Product.Id, isTracking: false);

                    if (files.Count > 0)
                    {
                        string upload = webRootPath + WC.ImagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(upload, objFromDb.Image);

                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productViewModel.Product.Image = fileName + extension;
                    }
                    else
                    {
                        productViewModel.Product.Image = objFromDb.Image;
                    }

                    _productRepository.Update(productViewModel.Product);
                }

                _productRepository.Save();
                return RedirectToAction("Index");
            }

            productViewModel.CategoryDropDownList = _productRepository.GetAllDropdownList(WC.CategoryName);
            productViewModel.ApplicationTypeDropDownList = _productRepository.GetAllDropdownList(WC.ApplicationTypeName);

            return View(productViewModel);
        }

        // GET - DELETE
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var product = _productRepository.FirstOrDefault(p => p.Id == id, includProps: $"{WC.CategoryName}, {WC.ApplicationTypeName}");

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST - DELETE
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _productRepository.GetById(id.GetValueOrDefault());

            if (obj == null)
            {
                return NotFound();
            }

            string upload = _webHostEnvironment.WebRootPath + WC.ImagePath;
            var oldFile = Path.Combine(upload, obj.Image);

            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }

            _productRepository.Remove(obj);
            _productRepository.Save();
            return RedirectToAction("Index");
        }
    }
}