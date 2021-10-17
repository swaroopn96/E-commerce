using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ecommerce.Controllers
{
    public class ApplicationTypeController : Controller
    {
        private readonly ApplicationDbContext _db;

        //Here db will be sent by startup file which does the Dependency Injection
        public ApplicationTypeController(ApplicationDbContext db)
        {
            _db = db;
        }


        // GET: /<controller>/
        public IActionResult Index()
        {
            IEnumerable<ApplicationType> applicationTypeList = _db.ApplicationType;

            return View(applicationTypeList);
        }

        //GET-Create
        public IActionResult Create()
        {
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken] //It validates that the tocken is still valid and security is not tampered
        public IActionResult Create(ApplicationType obj)
        {
            _db.ApplicationType.Add(obj);
            _db.SaveChanges();//here it updates db
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var obj = _db.ApplicationType.Find(id);

            if (obj == null)
                return NotFound();

            return View(obj);

        }

        [HttpPost]
        [ValidateAntiForgeryToken] //It validates that the tocken is still valid and security is not tampered
        public IActionResult Edit(ApplicationType obj)
        {
            //ModelState checks whether all the conditions specified are met
            if (ModelState.IsValid)
            {
                _db.ApplicationType.Update(obj);
                _db.SaveChanges();//here it updates db
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //GET-DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var obj = _db.ApplicationType.Find(id);

            if (obj == null)
                return NotFound();

            return View(obj);

        }

        [HttpPost]
        [ValidateAntiForgeryToken] //It validates that the tocken is still valid and security is not tampered
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.ApplicationType.Find(id);

            //ModelState checks whether all the conditions specified are met
            if (obj == null)
                return NotFound();

            _db.ApplicationType.Remove(obj);
            _db.SaveChanges();//here it updates db
            return RedirectToAction("Index");
        }
    }
}
