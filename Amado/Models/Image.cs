using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Amado.Models
{
    public class Image
    {
        [Key]
        [Required]
        public int ImageID { get; set; }

        [ForeignKey("ItemModel")]
        public int ItemID { get; set; }

        [DefaultValue("/assets/default.png")]
        public String? ImageURL { get; set; }

        public Item? Item { get; set; }
    }
}
