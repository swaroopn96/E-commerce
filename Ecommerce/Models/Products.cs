using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models
{
    public class Products
    {
        public Products()
        {
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string ShortDesc { get; set; }

        [Range(1, int.MaxValue)]
        public double Price { get; set; }

        public string Image { get; set; }

        [Display(Name = "Category Type")]
        public int CategoryId { get; set; }
        //It adds foreign key i.e it creates relation between product and category table
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [Display(Name = "Application Type")]
        public int ApplicationTypeId { get; set; }

        [ForeignKey("ApplicationTypeId")]
        public virtual ApplicationType ApplicationType { get; set; }
    }
}
