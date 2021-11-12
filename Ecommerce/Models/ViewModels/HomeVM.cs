using System;
using System.Collections.Generic;

namespace Ecommerce.Models.ViewModels
{
    public class HomeVM
    {
        public HomeVM()
        {
        }

        public IEnumerable<Products> Products { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
