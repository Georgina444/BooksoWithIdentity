using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookso.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        [Display(Name = "List Price")]
        public double ListPrice { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        [Display(Name = "Price for 2+")]

        public double Price2 { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }

        // Foreign keys

        [Required]
        [Display(Name = "Category")]

        public int CategoryId { get; set; }
        // [ForeignKey("CategoryID")]
        [ValidateNever]
        public Category Category { get; set; }    

        [Required]
        [Display(Name ="Cover Type")]
        public int CoverTypeId { get; set; }      // Automatically creates a foreign key relation if the name is the same and Id is added
        [ValidateNever]
        public CoverType CoverType { get; set; }


    }
}
