using System;
namespace Ecommerce.Models.ViewModels
{
    public class DetailsVM
    {
        public DetailsVM()
        {
            Product = new Products(); //If we do here no need to do in consuming controller
        }

        public Products Product { get; set; }

        public bool ExistsInCart { get; set; }
    }
}
