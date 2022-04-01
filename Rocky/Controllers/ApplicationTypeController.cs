using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rocky.DAL.Repository.Interfaces;
using Rocky.Domain;
using Rocky.Utils;
using System.Linq;

namespace Rocky.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class ApplicationTypeController : Controller
    {
        private readonly IApplicationTypeRepository _appTypeRepository;

        public ApplicationTypeController(IApplicationTypeRepository repository)
        {
            _appTypeRepository = repository;
        }

        public IActionResult Index()
        {
            IQueryable<ApplicationType> applicationTypes = _appTypeRepository.GetAll();
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

            _appTypeRepository.Add(applicationType);
            _appTypeRepository.Save();
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

            var appType = _appTypeRepository.GetById(id.GetValueOrDefault());

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

            _appTypeRepository.Update(applicationType);
            _appTypeRepository.Save();
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

            var appType = _appTypeRepository.GetById(id.GetValueOrDefault());

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
            var appType = _appTypeRepository.GetById(id.GetValueOrDefault());

            if (appType == null)
            {
                return NotFound();
            }

            _appTypeRepository.Remove(appType);
            _appTypeRepository.Save();
            return RedirectToAction("Index");
        }
    }
}