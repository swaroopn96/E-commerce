using System;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    public class ApplicationType
    {
        public ApplicationType()
        {
        }

        [Key] //This key makes this Id primary and identity
        public int Id { get; set; }

        public string Name { get; set; }

    }
}
