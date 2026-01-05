using App_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace App_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {



        // [Authorize]
        [HttpGet]
        public async Task<IEnumerable<Cart>> Get()
        {
            using (FoodDbContext entities = new FoodDbContext())
            {
                return await entities.Cart.ToListAsync();
            }
        }



        // [Authorize]
        [HttpGet("{id}", Name = "GetCartId")]
        public async Task<IActionResult> GetCartId(string id)
        {
            List<Cart> clist = new List<Cart>();
            using (FoodDbContext entities = new FoodDbContext())
            {
                clist = await entities.Cart.ToListAsync();
                var Check = clist.FindAll(uid => uid.LoginId == id);
                if (Check != null)
                {
                    return Ok(Check);
                }
                else
                {
                    return NotFound("Not Found.");
                }
            }
        }



        ////[Authorize]
        //[HttpPost]
        //public async Task<JsonResult> Post([FromBody] CartValue c)



        //{
        // FoodOrderContext db = new FoodOrderContext();



        // try
        // {
        // db.CartValues.Add(c);
        // db.SaveChanges();
        // return new JsonResult("Food Added to the Cart SuccessFully");
        // }
        // catch (Exception)
        // {
        // return new JsonResult("Food Item Cannot be added");
        // }





        //[Authorize]
        [HttpDelete("{cid}")]
        public async Task<JsonResult> Delete(int cid)
        {
            try
            {
                using (FoodDbContext entities = new FoodDbContext())
                {
                    var entity = await entities.Cart.SingleOrDefaultAsync(cart => cart.CartId == cid);
                    if (entity == null)
                    {
                        return new JsonResult("Unable to Remove Food Item from Cart.");
                    }
                    else
                    {
                        entities.Cart.Remove(entity);
                        await entities.SaveChangesAsync();
                        return new JsonResult("Food Item Removed from Cart.");
                    }
                }
            }
            catch (Exception)
            {
                return new JsonResult("Cart Item not found.");
            }
        }
        // POST: api/Carts
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<JsonResult> Post([FromBody] Cart c)
        {
           
                using (FoodDbContext entities = new FoodDbContext())
                {
                    if (entities.Cart.Where(cart => cart.FoodCode == c.FoodCode && cart.LoginId == c.LoginId).Count() > 0)
                    {
                        var cartData = entities.Cart.Where(cart => cart.FoodCode == c.FoodCode && cart.LoginId == c.LoginId).FirstOrDefault();
                        cartData.Quantity = cartData.Quantity + 1;
                        entities.Cart.Update(cartData);
                        await entities.SaveChangesAsync();
                        return new JsonResult("Food added to cart.");
                    }
                    else
                    {
                        entities.Cart.Add(c);
                        await entities.SaveChangesAsync();
                        return new JsonResult("Food added to cart.");
                    }





                }
            
           
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Cart cart)

        {
            FoodDbContext db = new FoodDbContext();
            try
            {
                var entity = db.Cart.FirstOrDefault(c => c.CartId == id);
                if (entity == null)
                {
                    return NotFound("Not Found.");
                }
                else
                {
                    entity.Quantity = cart.Quantity;
                    //entity.TotalPrice = cart.Quantity * entity.Price;
                    db.SaveChanges(); return Ok();
                }
            }
            catch (Exception)
            {
                return new JsonResult("Unable to Update to the cart.");
            }
        }

        }


    }