using System;
using System.Collections.Generic;

namespace Ecommerce.Models.ViewModels
{
    public class ProductUserVM
    {
        public ProductUserVM()
        {
            ProductList = new List<Products>();
        }

        public ApplicationUser ApplicationUser { get; set; }

        public IEnumerable<Products> ProductList { get; set; }
    }
}
