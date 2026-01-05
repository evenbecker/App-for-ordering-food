using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace App_API.Models
{
    public partial class UserDetails
    {
        public UserDetails()
        {
            Cart = new HashSet<Cart>();
            FoodOrder = new HashSet<FoodOrder>();
        }

        public string LoginId { get; set; }
        public string UserName { get; set; }
        public long Phone { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Cart> Cart { get; set; }
        public virtual ICollection<FoodOrder> FoodOrder { get; set; }
    }
}
