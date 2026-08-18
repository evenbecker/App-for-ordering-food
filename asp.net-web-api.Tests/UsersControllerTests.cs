using asp.net_web_api.Controllers;
using asp.net_web_api.Data;
using asp.net_web_api.Dtos;
using asp.net_web_api.Helpers;
using asp.net_web_api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

namespace asp.net_web_api.Tests
{
    public class UsersControllerTests
    {
        private FoodShopDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<FoodShopDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new FoodShopDbContext(options);

            // Seed test data
            var testUser = new User
            {
                Memberno = 1,
                Username = "testuser",
                Password = PasswordHasher.Md5Hash("password123"), // Pre-hashed
                Forename = "John",
                Surname = "Doe",
                Email = "john.doe@example.com",
                Street = "123 Test St",
                Town = "Test Town",
                Postcode = "12345",
                Category = "Bronze"
            };

            context.Users.Add(testUser);
            context.SaveChanges();

            return context;
        }

        private void SetupControllerContext(UsersController controller, string? username = null, string? userId = null)
        {
            var claims = new List<Claim>();
            if (!string.IsNullOrEmpty(username))
            {
                claims.Add(new Claim(ClaimTypes.Name, username));
            }
            if (!string.IsNullOrEmpty(userId))
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, userId));
            }

            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = claimsPrincipal
                }
            };
        }

        [Fact]
        public void Login_ValidCredentials_ReturnsOkWithToken()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new UsersController(context);
            var loginRequest = new LoginRequest
            {
                Username = "testuser",
                Password = "password123"
            };

            // Act
            var result = controller.Login(loginRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            // Serialize to JSON to test anonymous object
            var jsonString = JsonSerializer.Serialize(okResult.Value);
            var jsonDocument = JsonDocument.Parse(jsonString);
            Assert.True(jsonDocument.RootElement.TryGetProperty("token", out var tokenProperty));
            Assert.False(string.IsNullOrEmpty(tokenProperty.GetString()));
        }

        [Fact]
        public void Login_InvalidUsername_ReturnsUnauthorized()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new UsersController(context);
            var loginRequest = new LoginRequest
            {
                Username = "nonexistentuser",
                Password = "password123"
            };

            // Act
            var result = controller.Login(loginRequest);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, unauthorizedResult.StatusCode);
            Assert.Equal("User not found", unauthorizedResult.Value);
        }

        [Fact]
        public void Login_InvalidPassword_ReturnsUnauthorized()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new UsersController(context);
            var loginRequest = new LoginRequest
            {
                Username = "testuser",
                Password = "wrongpassword"
            };

            // Act
            var result = controller.Login(loginRequest);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, unauthorizedResult.StatusCode);
            Assert.Equal("Invalid password", unauthorizedResult.Value);
        }

        [Fact]
        public void GetMyProfile_AuthenticatedUser_ReturnsOkWithUserData()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new UsersController(context);
            SetupControllerContext(controller, "testuser", "1");

            // Act
            var result = controller.GetMyProfile();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            // Serialize to JSON to test anonymous object
            var jsonString = JsonSerializer.Serialize(okResult.Value);
            var jsonDocument = JsonDocument.Parse(jsonString);
            
            Assert.True(jsonDocument.RootElement.TryGetProperty("Memberno", out var membernoProperty));
            Assert.Equal(1, membernoProperty.GetInt32());
            
            Assert.True(jsonDocument.RootElement.TryGetProperty("Username", out var usernameProperty));
            Assert.Equal("testuser", usernameProperty.GetString());
            
            Assert.True(jsonDocument.RootElement.TryGetProperty("Forename", out var forenameProperty));
            Assert.Equal("John", forenameProperty.GetString());
            
            Assert.True(jsonDocument.RootElement.TryGetProperty("Surname", out var surnameProperty));
            Assert.Equal("Doe", surnameProperty.GetString());
            
            Assert.True(jsonDocument.RootElement.TryGetProperty("Email", out var emailProperty));
            Assert.Equal("john.doe@example.com", emailProperty.GetString());

            // Verify password is not included
            Assert.False(jsonDocument.RootElement.TryGetProperty("Password", out _));
        }

        [Fact]
        public void GetMyProfile_UnauthenticatedUser_ReturnsUnauthorized()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new UsersController(context);
            SetupControllerContext(controller); // No username provided

            // Act
            var result = controller.GetMyProfile();

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, unauthorizedResult.StatusCode);
            Assert.Equal("User is not authenticated", unauthorizedResult.Value);
        }

        [Fact]
        public void GetMyProfile_UserNotFoundInDatabase_ReturnsNotFound()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new UsersController(context);
            SetupControllerContext(controller, "nonexistentuser", "999");

            // Act
            var result = controller.GetMyProfile();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Equal("User not found", notFoundResult.Value);
        }

        [Fact]
        public void Register_ValidUser_ReturnsOkWithSuccessMessage()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new UsersController(context);
            var newUser = new User
            {
                Username = "newuser",
                Password = "newpassword123",
                Forename = "Jane",
                Surname = "Smith",
                Email = "jane.smith@example.com",
                Street = "456 New St",
                Town = "New Town",
                Postcode = "67890",
                Category = "Silver"
            };

            // Act
            var result = controller.Register(newUser);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("User registered successfully", okResult.Value);

            // Verify user was added to database with hashed password
            var savedUser = context.Users.FirstOrDefault(u => u.Username == "newuser");
            Assert.NotNull(savedUser);
            Assert.Equal("newuser", savedUser.Username);
            Assert.Equal(PasswordHasher.Md5Hash("newpassword123"), savedUser.Password);
            Assert.Equal("Jane", savedUser.Forename);
            Assert.Equal("Silver", savedUser.Category);
        }

        [Fact]
        public void Register_DuplicateUsername_ReturnsBadRequest()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new UsersController(context);
            var duplicateUser = new User
            {
                Username = "testuser", // Same as existing user
                Password = "password123",
                Forename = "Duplicate",
                Surname = "User",
                Email = "duplicate@example.com"
            };

            // Act
            var result = controller.Register(duplicateUser);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("Username already exists", badRequestResult.Value);
        }

        [Fact]
        public void Register_NullUser_ReturnsBadRequest()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new UsersController(context);

            // Act
            var result = controller.Register(null!);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("Invalid user data", badRequestResult.Value);
        }
    }
}
