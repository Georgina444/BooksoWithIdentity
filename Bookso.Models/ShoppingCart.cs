using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookso.Models
{
    public class ShoppingCart
    {
        public Product Product { get; set; }
        [Range(1, 100, ErrorMessage = "Sorry, you cannot order more that 100 books at a time.")]
        public int Count { get; set; }
    }
}
