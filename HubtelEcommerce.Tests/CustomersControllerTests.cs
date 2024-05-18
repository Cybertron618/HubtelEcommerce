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
    public class CustomersControllerTests : IDisposable
    {
        private readonly EcommerceDbContext _context;

        public CustomersControllerTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var options = new DbContextOptionsBuilder<EcommerceDbContext>()
                .UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                .Options;

            _context = new EcommerceDbContext(options);

            // Seed some test data
            _context.Customers.Add(new Customer { ID = 1, Name = "Test Customer", Email = "test@example.com" });
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetCustomers_ReturnsOkResult()
        {
            // Arrange
            var controller = new CustomersController(_context);

            // Act
            var result = await controller.GetCustomers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var customers = Assert.IsAssignableFrom<IEnumerable<Customer>>(okResult.Value);
            Assert.Single(customers); // Assuming one customer in the test data
        }

        [Fact]
        public async Task GetCustomer_ReturnsOkResult()
        {
            // Arrange
            var controller = new CustomersController(_context);

            // Act
            var result = await controller.GetCustomer(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var customer = Assert.IsType<Customer>(okResult.Value);
            Assert.Equal("Test Customer", customer.Name);
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
