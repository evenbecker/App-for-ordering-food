using System;
using System.Collections.Generic;

#nullable disable

namespace App_API.Models
{
    public partial class UserDetail
    {
        public string LoginId { get; set; }
        public string UserName { get; set; }
        public long Phone { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
    }
}
