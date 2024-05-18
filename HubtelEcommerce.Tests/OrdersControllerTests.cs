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
    public class OrdersControllerTests : IDisposable
    {
        private readonly EcommerceDbContext _context;

        public OrdersControllerTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var options = new DbContextOptionsBuilder<EcommerceDbContext>()
                .UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                .Options;

            _context = new EcommerceDbContext(options);

            // Seed some test data
            _context.Orders.Add(new Order { ID = 1, CustomerID = 1, OrderDate = DateTime.Now, Total = 100 });
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetOrders_ReturnsOkResult()
        {
            // Arrange
            var controller = new OrdersController(_context);

            // Act
            var result = await controller.GetOrders();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var orders = Assert.IsAssignableFrom<IEnumerable<Order>>(okResult.Value);
            Assert.Single(orders); // Assuming one order in the test data
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
