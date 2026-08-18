using asp.net_web_api.Controllers;
using asp.net_web_api.Data;
using asp.net_web_api.Dtos;
using asp.net_web_api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

namespace asp.net_web_api.Tests
{
    public class OrdersControllerTests
    {
        private FoodShopDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<FoodShopDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new FoodShopDbContext(options);
        }

        private void SetupControllerContext(OrdersController controller, string username)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username)
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = principal }
            };
        }

        [Fact]
        public void CreateOrder_ReturnsUnauthorized_WhenUserNotAuthenticated()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new OrdersController(context);
            
            // Set up controller context with no user (simulating unauthenticated request)
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
            };
            
            var orderItems = new List<OrderLineDTO>
            {
                new OrderLineDTO { Stockno = "TEST1", Amount = 1 }
            };

            // Act
            var result = controller.CreateOrder(orderItems);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Invalid user session", unauthorizedResult.Value);
        }

        [Fact]
        public void CreateOrder_ReturnsBadRequest_WhenNoItemsProvided()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new OrdersController(context);
            SetupControllerContext(controller, "testuser");

            // Act
            var result = controller.CreateOrder(new List<OrderLineDTO>());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("No products selected", badRequestResult.Value);
        }

        [Fact]
        public void CreateOrder_ReturnsOk_WithValidOrder()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            
            // Setup test data
            var testUser = new User 
            { 
                Memberno = 1, 
                Username = "testuser",
                Forename = "Test",
                Surname = "User",
                Password = "password",
                Street = "123 Test St",
                Town = "Test Town",
                Postcode = "12345",
                Email = "test@test.com",
                Category = "gold"
            };
            var testProduct = new Product 
            { 
                Stockno = "TEST1", 
                Description = "Test Product", 
                Price = 100.0, 
                Qtyinstock = 10 
            };

            context.Users.Add(testUser);
            context.Products.Add(testProduct);
            context.SaveChanges();

            var controller = new OrdersController(context);
            SetupControllerContext(controller, "testuser");

            var orderItems = new List<OrderLineDTO>
            {
                new OrderLineDTO { Stockno = "TEST1", Amount = 2 }
            };

            // Act
            var result = controller.CreateOrder(orderItems);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            
            // Verify order was created
            var order = context.Orders.First();
            Assert.Equal(1, order.Memberno);

            // Verify order line was created
            var orderLine = context.OrderLines.First();
            Assert.Equal("TEST1", orderLine.Stockno);
            Assert.Equal(2, orderLine.Quantity);

            // Verify stock was updated
            var updatedProduct = context.Products.First(p => p.Stockno == "TEST1");
            Assert.Equal(8, updatedProduct.Qtyinstock); // 10 - 2 = 8
        }

        [Fact]
        public void CreateOrder_ReturnsBadRequest_WhenInsufficientStock()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            
            var testUser = new User 
            { 
                Memberno = 1, 
                Username = "testuser",
                Forename = "Test",
                Surname = "User",
                Password = "password",
                Street = "123 Test St",
                Town = "Test Town",
                Postcode = "12345",
                Email = "test@test.com",
                Category = "gold"
            };
            var testProduct = new Product 
            { 
                Stockno = "TEST1", 
                Description = "Test Product", 
                Price = 100.0, 
                Qtyinstock = 1  // Only 1 in stock
            };

            context.Users.Add(testUser);
            context.Products.Add(testProduct);
            context.SaveChanges();

            var controller = new OrdersController(context);
            SetupControllerContext(controller, "testuser");

            var orderItems = new List<OrderLineDTO>
            {
                new OrderLineDTO { Stockno = "TEST1", Amount = 5 } // Requesting more than available
            };

            // Act
            var result = controller.CreateOrder(orderItems);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Not enough stock", badRequestResult.Value?.ToString());
        }

        [Fact]
        public void GetOrders_ReturnsUnauthorized_WhenUserNotAuthenticated()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new OrdersController(context);
            
            // Set up controller context with no user (simulating unauthenticated request)
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var result = controller.GetOrders();

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Invalid user session", unauthorizedResult.Value);
        }

        [Fact]
        public void GetOrders_ReturnsOk_WithUserOrders()
        {
            // Arrange
            using var context = GetInMemoryDbContext();

            // Setup test data
            var testUser = new User
            {
                Memberno = 1,
                Username = "testuser",
                Forename = "Test",
                Surname = "User",
                Password = "password",
                Street = "123 Test St",
                Town = "Test Town",
                Postcode = "12345",
                Email = "test@test.com",
                Category = "gold"
            };
            var testProduct = new Product
            {
                Stockno = "TEST1",
                Description = "Test Product",
                Price = 100.0,
                Qtyinstock = 10
            };
            var testOrder = new Order { Orderno = 1, Memberno = 1 };
            var testOrderLine = new OrderLine { Orderno = 1, Stockno = "TEST1", Quantity = 2 };

            context.Users.Add(testUser);
            context.Products.Add(testProduct);
            context.Orders.Add(testOrder);
            context.OrderLines.Add(testOrderLine);
            context.SaveChanges();

            var controller = new OrdersController(context);
            SetupControllerContext(controller, "testuser");

            // Act
            var result = controller.GetOrders();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);

            // Serialize and deserialize to properly test the anonymous object structure
            var json = JsonSerializer.Serialize(okResult.Value);
            var orderData = JsonSerializer.Deserialize<JsonElement>(json);
            
            // Verify we have orders
            Assert.True(orderData.ValueKind == JsonValueKind.Array);
            Assert.Equal(1, orderData.GetArrayLength());
            
            // Check the first order
            var firstOrder = orderData[0];
            Assert.True(firstOrder.TryGetProperty("Orderno", out var orderNo));
            Assert.Equal(1, orderNo.GetInt32());
            
            Assert.True(firstOrder.TryGetProperty("TotalPrice", out var totalPrice));
            Assert.Equal(200.0, totalPrice.GetDouble()); // 100 * 2 = 200
            
            Assert.True(firstOrder.TryGetProperty("Items", out var items));
            Assert.True(items.ValueKind == JsonValueKind.Array);
            Assert.Equal(1, items.GetArrayLength());
            
            // Check the first item
            var firstItem = items[0];
            Assert.True(firstItem.TryGetProperty("Stockno", out var stockno));
            Assert.Equal("TEST1", stockno.GetString());
            
            Assert.True(firstItem.TryGetProperty("Quantity", out var quantity));
            Assert.Equal(2, quantity.GetInt32());
            
            Assert.True(firstItem.TryGetProperty("Description", out var description));
            Assert.Equal("Test Product", description.GetString());
            
            Assert.True(firstItem.TryGetProperty("Price", out var price));
            Assert.Equal(100.0, price.GetDouble());
        }

        [Fact]
        public void GetOrders_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new OrdersController(context);
            SetupControllerContext(controller, "nonexistentuser");

            // Act
            var result = controller.GetOrders();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User not found", notFoundResult.Value);
        }
    }
}
