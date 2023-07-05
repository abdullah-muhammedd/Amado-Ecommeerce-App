using Amado.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace Amado.Models
{
    public class Item
    {
        [Key]
        [Required]
        public int ItemID { get; set; }
        public string? Name { get; set; }
        public double Price { get; set; }
        public double Rate { get; set; }
        public Category? Category { get; set; }
        public Brand? Brand { get; set; }

        public Color Color { get; set; }

        public int Quantity { get; set; }

        public List<Cart_Item>? CartItems { get; set; }
        public List<Image>? Images { get; set; }
    }
}
