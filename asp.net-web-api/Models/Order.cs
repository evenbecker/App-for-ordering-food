using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace asp.net_web_api.Models
{
    [Table("EORDER")]
    public class Order
    {
        [Key]
        public int Orderno { get; set; } 
        public int Memberno { get; set; }
    }
}
