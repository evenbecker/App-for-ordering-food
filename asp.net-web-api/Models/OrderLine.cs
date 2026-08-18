using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace asp.net_web_api.Models
{
    [Table("EORDERLINE")]
    public class OrderLine
    {
        public int Orderno { get; set; }
        [StringLength(5)]
        public string Stockno { get; set; }
        public int Quantity { get; set; }
    }
}
