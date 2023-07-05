using System.ComponentModel.DataAnnotations;

namespace Amado.Models
{
    public class Cart
    {
        [Key]
        [Required]
        public int CartID { get; set; }

        public double TotalPrice { get; set; }

        public List<Cart_Item>? CartItems { get; set; }
        public List<Order>? Orders { get; set; }

    }
}
