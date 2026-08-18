using asp.net_web_api.Data;
using asp.net_web_api.Dtos;
using asp.net_web_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace asp.net_web_api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly FoodShopDbContext context;
        public OrdersController(FoodShopDbContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public IActionResult CreateOrder([FromBody] List<OrderLineDTO> items)
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                return Unauthorized("Invalid user session");

            if (items == null || items.Count == 0)
                return BadRequest("No products selected");

            var user = context.Users.SingleOrDefault(u => u.Username == username);

            // Create a new order
            var order = new Order
            {
                Memberno = user.Memberno
            };

            context.Orders.Add(order);
            context.SaveChanges(); // Save to get the Orderno

            foreach (var item in items)
            {
                //get the product from the database
                var product = context.Products.SingleOrDefault(s => s.Stockno == item.Stockno);

                if (product == null)
                    return NotFound($"Product with Stockno {product.Stockno} not found");

                if (product.Qtyinstock < item.Amount)    
                        return BadRequest($"Not enough stock for product {product.Description} (Stockno: {product.Stockno})");

                // Create order line
                var orderLine = new OrderLine
                {
                    Orderno = order.Orderno,
                    Stockno = item.Stockno,
                    Quantity = item.Amount
                };

                context.OrderLines.Add(orderLine);
                product.Qtyinstock -= item.Amount; // Update stock quantity
            }
            context.SaveChanges();
            return Ok(new { Message = "Order placed", Orderno = order.Orderno });
        }

        [HttpGet("allorders")]
        public IActionResult GetOrders()
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                return Unauthorized("Invalid user session");

            var user = context.Users.SingleOrDefault(u => u.Username == username);
            if (user == null)
                return NotFound("User not found");

            var orders = context.Orders
                .Where(o => o.Memberno == user.Memberno)
                .OrderByDescending(o => o.Orderno)
                .ToList();

            var result = orders.Select(order =>
            {
                var items = context.OrderLines
                    .Where(ol => ol.Orderno == order.Orderno)
                    .Join(context.Products,
                          ol => ol.Stockno,
                          p => p.Stockno,
                          (ol, p) => new
                          {
                              ol.Stockno,
                              ol.Quantity,
                              Description = p.Description,
                              Price = p.Price,
                              Total = p.Price * ol.Quantity
                          })
                    .ToList();

                var totalPrice = items.Sum(i => i.Total);

                return new
                {
                    order.Orderno,
                    Items = items.Select(i => new
                    {
                        i.Stockno,
                        i.Quantity,
                        i.Description,
                        i.Price
                    }),
                    TotalPrice = totalPrice
                };
            });

            return Ok(result);
        }
    }
}
