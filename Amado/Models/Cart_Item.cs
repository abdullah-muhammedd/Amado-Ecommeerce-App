using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Amado.Models
{
    public class Cart_Item
    {
        [Key]
        [ForeignKey("CartModel")]
        public int CartID { get; set; }

        public Cart? Cart { get; set; }

        [Key]
        [ForeignKey("ItemModel")]
        public int ItemID { get; set; }

        public Item? Item { get; set; }

        public int Quantity { get; set; }
    }
}
