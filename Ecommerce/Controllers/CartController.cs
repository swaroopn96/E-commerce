using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Data;
using Ecommerce.Models;
using Ecommerce.Models.ViewModels;
using Ecommerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ecommerce.Controllers
{
    [Authorize] //This Controller accessible only if the user is logged in
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;


        [BindProperty]
        public ProductUserVM ProductUserVM { get; set; }

        public CartController(ApplicationDbContext db,IWebHostEnvironment webHostEnvironment,IEmailSender emailSender)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();

            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstants.SessionCart);
            }

            List<int> prodInCart = shoppingCartList.Select(x => x.ProductId).ToList();

            IEnumerable<Products> prodList = _db.Products.Where(x => prodInCart.Contains(x.Id));

            return View(prodList);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            return RedirectToAction(nameof(Summary));
        }


        
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier); //if user logged in then claim has value else null

            //Also we can use
            //var userId = User.FindFirstValue(ClaimTypes.Name);

            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();

            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstants.SessionCart);
            }

            List<int> prodInCart = shoppingCartList.Select(x => x.ProductId).ToList();

            IEnumerable<Products> prodList = _db.Products.Where(x => prodInCart.Contains(x.Id));

            ProductUserVM = new ProductUserVM()
            {
                ApplicationUser = _db.ApplicationUser.FirstOrDefault(x => x.Id == claim.Value)
            };
            ProductUserVM.ProductList = prodList.ToList();

            return View(ProductUserVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPost(ProductUserVM productUserVM)  //Since we have used bindpropery in line 23 even if we don't pass parameter it works
        {
            var PathToTemplate = _webHostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                +"templates"+Path.DirectorySeparatorChar.ToString()+"Inquiry.html";
            var subject = "New Inquiry";
            string HtmlBody = "";

            using(StreamReader sr= System.IO.File.OpenText(PathToTemplate))
            {
                HtmlBody = sr.ReadToEnd();
            }

            StringBuilder prodListSB = new StringBuilder();
            foreach(var prod in ProductUserVM.ProductList)
            {
                prodListSB.Append($" - Name: {prod.Name} <span style='font-size:14px;'>(ID: {prod.Id})</span><br/>");
            }

            string messageBody = string.Format(HtmlBody,
                productUserVM.ApplicationUser.FullName,
                productUserVM.ApplicationUser.Email,
                productUserVM.ApplicationUser.PhoneNumber,
                prodListSB.ToString());

            await _emailSender.SendEmailAsync(WebConstants.AdminEmail, subject, messageBody);

            return RedirectToAction(nameof(InquiryConfirmation));

        }

        public IActionResult InquiryConfirmation()
        {
            HttpContext.Session.Clear();
            return View();
        }



        public IActionResult Remove(int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();

            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstants.SessionCart);
            }

            shoppingCartList.Remove(shoppingCartList.FirstOrDefault(x => x.ProductId == id));
            HttpContext.Session.Set(WebConstants.SessionCart, shoppingCartList);

            return RedirectToAction(nameof(Index));
        }
    }
}
