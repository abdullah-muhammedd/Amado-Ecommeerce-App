using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Amado.Models
{
    public class Order
    {
        [Key]
        [Required]
        public int OrderID { get; set; }
        [ForeignKey("UserModel")]
        public int UserID { get; set; }
        public User? User { get; set; }
        public string? Comment { get; set; }
        public double TotalPrice { get; set; }
    }
}
