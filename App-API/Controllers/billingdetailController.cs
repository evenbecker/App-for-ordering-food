using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App_API.Models;

namespace App_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class billingdetailController : ControllerBase
    {
        private readonly FoodDbContext _context;

        public billingdetailController(FoodDbContext context)
        {
            _context = context;
        }

        // GET: api/billingdetail
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodOrder>>> GetFoodOrders()
        {
            return await _context.FoodOrder.ToListAsync();
        }

        // GET: api/billingdetail/5
        [HttpGet("{id}", Name = "GetFoodOrderId")]
        public async Task<IActionResult> GetFoodOrder(string id)
        {
            List<FoodOrder> clist = new List<FoodOrder>();
            using (FoodDbContext entities = new FoodDbContext())
            {
                clist = await entities.FoodOrder.ToListAsync();
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

        // PUT: api/billingdetail/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFoodOrder(int id, FoodOrder foodOrder)
        {
            if (id != foodOrder.OrderNo)
            {
                return BadRequest();
            }

            _context.Entry(foodOrder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FoodOrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/billingdetail
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<FoodOrder>> PostFoodOrder(FoodOrder foodOrder)
        {
            _context.FoodOrder.Add(foodOrder);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFoodOrder", new { id = foodOrder.OrderNo }, foodOrder);
        }

        // DELETE: api/billingdetail/5
        [HttpDelete("{oid}")]
        public async Task<JsonResult> Delete(int oid)
        {
            try
            {
                using (FoodDbContext entities = new FoodDbContext())
                {
                    var entity = await entities.FoodOrder.SingleOrDefaultAsync(FoodOrder => FoodOrder.OrderNo == oid);
                    if (entity == null)
                    {
                        return new JsonResult("Unable to Remove Food Item from Bill.");
                    }
                    else
                    {
                        entities.FoodOrder.Remove(entity);
                        await entities.SaveChangesAsync();
                        return new JsonResult("Food Item Removed from Bill.");
                    }
                }
            }
            catch (Exception)
            {
                return new JsonResult("Bill not found.");
            }
        }

        private bool FoodOrderExists(int id)
        {
            return _context.FoodOrder.Any(e => e.OrderNo == id);
        }
    }
}
