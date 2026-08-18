using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace asp.net_web_api.Models
{
    [Table("ESTOCK")]
    public class Product
    {
        [Key]
        public string? Stockno { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public int Qtyinstock { get; set; }
    }
}
