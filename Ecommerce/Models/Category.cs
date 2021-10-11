using System;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    public class Category
    {
        public Category()
        {
        }

        [Key] //This key makes this Id primary and identity
        public int Id { get; set; }
        public string Name { get; set; }
        public int DisplayOrder { get; set; }

    }
}
