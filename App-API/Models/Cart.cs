using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace App_API.Models
{
    public partial class Cart
    {
        public int CartId { get; set; }
        public string LoginId { get; set; }
        public int? FoodCode { get; set; }
        public string FoodName { get; set; }
        public int? Price { get; set; }
        public int? Quantity { get; set; }

        public virtual FoodDetail FoodCodeNavigation { get; set; }
        public virtual UserDetails Login { get; set; }
    }
}
