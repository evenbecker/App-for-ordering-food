using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace asp.net_web_api.Models
{
    [Table("EMEMBER")]
    public class User
    {
        [Key]
        public int Memberno { get; set; }
        public string Username { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public string Street { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }
        public string Email { get; set; }
        public string Category { get; set; }

    }
}
