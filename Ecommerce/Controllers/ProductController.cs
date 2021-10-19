using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Data;
using Ecommerce.Models;
using Ecommerce.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ecommerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;

        //Here db will be sent by startup file which does the Dependency Injection
        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }

       
        // GET: /<controller>/
        public IActionResult Index()
        {
            IEnumerable<Products> productList = _db.Products;

            foreach(var obj in productList)
            {
                obj.Category = _db.Category.FirstOrDefault(x => x.Id == obj.CategoryId);
            }

            return View(productList);
        }


        //GET-Common method Upsert to update and create
        public IActionResult Upsert(int? id)
        {
            IEnumerable<SelectListItem> categoryDropDown = _db.Category.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });

            //using View Bag
            //ViewBag.CategoryDropDown = categoryDropDown;

            //Using View Data
            ViewData["CategoryDropDown"] = categoryDropDown;
            

            Products product = new Products();

            if (id==null)
            {
                //this is for create
                return View(product);
            }
            else
            {
                product = _db.Products.Find(id);
                if (product == null)
                    return NotFound();

                return View(product);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //It validates that the tocken is still valid and security is not tampered
        public IActionResult Create(Category obj)
        {
            //ModelState checks whether all the conditions specified are met
            if (ModelState.IsValid)
            {
                _db.Category.Add(obj);
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

            var obj = _db.Category.Find(id);

            if (obj == null)
                return NotFound();

            return View(obj);

        }

        [HttpPost]
        [ValidateAntiForgeryToken] //It validates that the tocken is still valid and security is not tampered
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Category.Find(id);

            //ModelState checks whether all the conditions specified are met
            if (obj == null)
                return NotFound();

            _db.Category.Remove(obj);
            _db.SaveChanges();//here it updates db
            return RedirectToAction("Index");
        }
    }
}
