using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApp.Controllers;
using WebApp.Data;
using WebApp.Models;
using Xunit;

namespace WebApp.Tests
{
    public class CategoriesControllerTests
    {
        [Fact]
        public async Task Create_ValidModelState_RedirectsToIndex()
        {
            var mockContext = new Mock<ApplicationDbContext>(); // Replace YourDbContext with your actual DbContext type
            var controller = new CategoriesController(mockContext.Object);
            var category = new Category { Name = "Test Category" };

            var result = await controller.Create(category) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Create_InvalidModelState_ReturnsViewResult()
        {
            // Arrange
            var mockContext = new Mock<ApplicationDbContext>(); // Replace YourDbContext with your actual DbContext type
            var controller = new CategoriesController(mockContext.Object);
            var category = new Category();
            controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await controller.Create(category) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(category, result.Model);
        }
    }
}
