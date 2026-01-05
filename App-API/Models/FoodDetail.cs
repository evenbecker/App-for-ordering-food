using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace App_API.Models
{
    public partial class FoodDetail
    {
        public FoodDetail()
        {
            Cart = new HashSet<Cart>();
            FoodOrder = new HashSet<FoodOrder>();
        }

        public int FoodCode { get; set; }
        public string FoodName { get; set; }
        public string Description { get; set; }
        public int? Price { get; set; }

        public virtual ICollection<Cart> Cart { get; set; }
        public virtual ICollection<FoodOrder> FoodOrder { get; set; }
    }
}
