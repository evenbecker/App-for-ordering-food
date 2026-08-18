using asp.net_web_api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace asp.net_web_api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly FoodShopDbContext context;

        public ProductsController(FoodShopDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAllProducts()
        {
            var products = context.Products.ToList();
            return Ok(products);
        }
    }
}
