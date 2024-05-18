using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HubtelEcommerce.Api.Controllers;
using HubtelEcommerce.Api.DataContext;
using HubtelEcommerce.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace HubtelEcommerce.Tests
{
    public class ProductsControllerTests : IDisposable
    {
        private readonly EcommerceDbContext _context;

        public ProductsControllerTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var options = new DbContextOptionsBuilder<EcommerceDbContext>()
                .UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                .Options;

            _context = new EcommerceDbContext(options);

            // Seed some test data
            _context.Products.Add(new Product { ID = 1, Name = "Test Product", Description = "Sample description", Price = 100 });
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetProducts_ReturnsOkResult()
        {
            // Arrange
            var controller = new ProductsController(_context);

            // Act
            var result = await controller.GetProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var products = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
            Assert.Single(products); // Assuming one product in the test data
        }

        // Implement IDisposable to dispose the context
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();

            // Suppress finalization to prevent the need for derived types to re-implement IDisposable
            GC.SuppressFinalize(this);
        }

    }
}
