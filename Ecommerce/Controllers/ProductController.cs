using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Data;
using Ecommerce.Models;
using Ecommerce.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ecommerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;

        private readonly IWebHostEnvironment _webHostEnvironment;
        private string fileName;


        //Here db will be sent by startup file which does the Dependency Injection
        public ProductController(ApplicationDbContext db,IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

       
        // GET: /<controller>/
        public IActionResult Index()
        {
            IEnumerable<Products> productList = _db.Products.Include(x=>x.Category).Include(x=>x.ApplicationType);

            //foreach(var obj in productList)
            //{
            //    obj.Category = _db.Category.FirstOrDefault(x => x.Id == obj.CategoryId);
            //    obj.ApplicationType = _db.ApplicationType.FirstOrDefault(x => x.Id == obj.ApplicationTypeId);
            //}

            return View(productList);
        }


        //GET-Common method Upsert to update and create
        public IActionResult Upsert(int? id)
        {
            /*IEnumerable<SelectListItem> categoryDropDown = _db.Category.Select(x => new SelectListItem
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
            }*/

            //Using ViewModel Intead of ViewBag or ViewData to pass data
            ProductVM productVM = new ProductVM()
            {
                Product = new Products(),
                CategorySelectList = _db.Category.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                ApplicationTypeSelectList = _db.ApplicationType.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            if (id==null)
            {
                //this is for create
                return View(productVM);
            }
            else
            {
                productVM.Product = _db.Products.Find(id);
                if (productVM.Product == null)
                    return NotFound();

                return View(productVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //It validates that the tocken is still valid and security is not tampered
        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {

                var files = HttpContext.Request.Form.Files; //To retrive image uploaded
                string webRootPath = _webHostEnvironment.WebRootPath;

                if(productVM.Product.Id==0)
                {
                    //Creating
                    string upload = webRootPath + WebConstants.ImagePath; //Path to upload image
                    string fileName = Guid.NewGuid().ToString(); //It generates random file name
                    string extension = Path.GetExtension(files[0].FileName); //Gets the extension of image

                    using(var filestream=new FileStream(Path.Combine(upload,fileName+extension),FileMode.Create))
                    {
                        files[0].CopyTo(filestream);
                    }

                    productVM.Product.Image = fileName + extension;

                    _db.Products.Add(productVM.Product);
                }
                else
                {
                    //Updating
                    //We get below error so use
                    //The instance of entity type 'Products' cannot be tracked because another instance with the same key value for {'Id'} is already being tracked.

                    var objFromDb = _db.Products.AsNoTracking().FirstOrDefault(x => x.Id == productVM.Product.Id);


                    //To check whether new image updated
                    if(files.Count>0)
                    {
                        string upload = webRootPath + WebConstants.ImagePath; //Path to upload image
                        string fileName = Guid.NewGuid().ToString(); //It generates random file name
                        string extension = Path.GetExtension(files[0].FileName); //Gets the extension of image

                        //To remove old file
                        var oldFile = Path.Combine(upload, objFromDb.Image);

                        if(System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        using (var filestream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(filestream);
                        }

                        productVM.Product.Image = fileName + extension;
                    }
                    else
                    {
                        productVM.Product.Image = objFromDb.Image;
                    }

                    _db.Products.Update(productVM.Product);
                }

                _db.SaveChanges();//here it updates db
                return RedirectToAction("Index");
            }
            productVM.CategorySelectList = _db.Category.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            productVM.ApplicationTypeSelectList = _db.ApplicationType.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            return View(productVM);
        }

        //GET-DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            //Products product = _db.Products.Find(id);
            //product.Category = _db.Category.Find(product.Category.Id);

            //We can automatically include category while loading product(eager loading) using below code instead of above 2
            Products product = _db.Products.Include(x=>x.Category).Include(x=>x.ApplicationType).FirstOrDefault(x=>x.Id==id);

            if (product == null)
                return NotFound();

            return View(product);

        }

        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken] //It validates that the tocken is still valid and security is not tampered
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Products.Find(id);

            //ModelState checks whether all the conditions specified are met
            if (obj == null)
                return NotFound();

            string upload = _webHostEnvironment.WebRootPath + WebConstants.ImagePath; //Path to upload image

            //To remove old file
            var oldFile = Path.Combine(upload, obj.Image);

            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }

            _db.Products.Remove(obj);
            _db.SaveChanges();//here it updates db
            return RedirectToAction("Index");
        }
    }
}
