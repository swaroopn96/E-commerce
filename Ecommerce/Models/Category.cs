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

        [Required]
        public string Name { get; set; }

        [DisplayName("Display Order")]
        [Required]
        [Range(1,int.MaxValue,ErrorMessage ="Display order for category must be greater than 0")]
        public int DisplayOrder { get; set; }

    }
}
