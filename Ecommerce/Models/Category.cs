using System;
using System.ComponentModel;
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

        [DisplayName("Display Order")]
        public int DisplayOrder { get; set; }

    }
}
