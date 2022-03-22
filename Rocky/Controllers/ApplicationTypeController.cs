using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rocky.Data;
using Rocky.Models;
using System.Collections.Generic;

namespace Rocky.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class ApplicationTypeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ApplicationTypeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<ApplicationType> applicationTypes = _db.ApplicationType;
            return View(applicationTypes);
        }
        
        // GET - CREATE
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ApplicationType applicationType)
        {
            if (!ModelState.IsValid)
            {
                return View(applicationType);
            }

            _db.ApplicationType.Add(applicationType);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET - EDIT
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null && id == 0)
            {
                return NotFound();
            }

            var appType = _db.ApplicationType.Find(id);

            if (appType == null)
            {
                return NotFound();
            }

            return View(appType);
        }

        // POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ApplicationType applicationType)
        {
            if (!ModelState.IsValid)
            {
                return View(applicationType);
            }

            _db.ApplicationType.Update(applicationType);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET - DELETE
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var appType = _db.ApplicationType.Find(id);

            if (appType == null)
            {
                return NotFound();
            }

            return View(appType);
        }

        // POST - DELETE
        [HttpPost]
        public IActionResult DeletePost(int? id)
        {
            var appType = _db.ApplicationType.Find(id);

            if (appType == null)
            {
                return NotFound();
            }

            _db.ApplicationType.Remove(appType);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}