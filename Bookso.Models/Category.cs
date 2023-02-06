using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bookso.Models
{
    public class Category
    {
        // CategoryID = Id (in his project)
        [Key] // primary key  (This is data annotation)
        public int CategoryID { get; set; }
        [Required]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "The Display order must be between 1 and 100 only!")]
        public int DisplayOrder { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;  // Automatically fills the date with current date(when it is created)
    }
}
