using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Amado.Models
{
    [PrimaryKey("OrderID" , "ItemID")]
    public class Order_Item
    {

        [ForeignKey("OrderModel")]
        public int OrderID { get; set; }

        public Order? Order { get; set; }

        public int ItemID { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public int Quantity { get; set; }
    }
}
