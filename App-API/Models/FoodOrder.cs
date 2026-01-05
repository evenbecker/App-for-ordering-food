using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace App_API.Models
{
    public partial class FoodOrder
    {
        public int OrderNo { get; set; }
        public string LoginId { get; set; }
        public int? FoodCode { get; set; }
        public string FoodName { get; set; }
        public int? BillingAmount { get; set; }
        public string Address { get; set; }

        public virtual FoodDetail FoodCodeNavigation { get; set; }
        public virtual UserDetails Login { get; set; }
    }
}
