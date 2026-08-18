using asp.net_web_api.Controllers;
using asp.net_web_api.Data;
using asp.net_web_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace asp.net_web_api.Tests
{
    public class ProductsControllerTests
    {
        private FoodShopDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<FoodShopDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new FoodShopDbContext(options);
        }

        [Fact]
        public void GetAllProducts_ReturnsOkResult_WithListOfProducts()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var testProducts = new List<Product>
            {
                new Product { Stockno = "TEST1", Description = "Test Product 1", Price = 100.0, Qtyinstock = 10 },
                new Product { Stockno = "TEST2", Description = "Test Product 2", Price = 200.0, Qtyinstock = 5 }
            };

            context.Products.AddRange(testProducts);
            context.SaveChanges();

            var controller = new ProductsController(context);

            // Act
            var result = controller.GetAllProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var products = Assert.IsAssignableFrom<List<Product>>(okResult.Value);
            Assert.Equal(2, products.Count);
            Assert.Equal("TEST1", products[0].Stockno);
            Assert.Equal("TEST2", products[1].Stockno);
        }

        [Fact]
        public void GetAllProducts_ReturnsOkResult_WithEmptyList_WhenNoProducts()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new ProductsController(context);

            // Act
            var result = controller.GetAllProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var products = Assert.IsAssignableFrom<List<Product>>(okResult.Value);
            Assert.Empty(products);
        }

        [Fact]
        public void GetAllProducts_ReturnsProducts_WithCorrectProperties()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var testProduct = new Product 
            { 
                Stockno = "SP123", 
                Description = "Super Product", 
                Price = 299.99, 
                Qtyinstock = 15 
            };

            context.Products.Add(testProduct);
            context.SaveChanges();

            var controller = new ProductsController(context);

            // Act
            var result = controller.GetAllProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var products = Assert.IsAssignableFrom<List<Product>>(okResult.Value);
            var product = products.First();
            
            Assert.Equal("SP123", product.Stockno);
            Assert.Equal("Super Product", product.Description);
            Assert.Equal(299.99, product.Price);
            Assert.Equal(15, product.Qtyinstock);
        }
    }
}
