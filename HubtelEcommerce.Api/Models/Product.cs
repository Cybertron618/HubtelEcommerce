﻿namespace HubtelEcommerce.Api.Models
{
    public class Product
    {
        public int ID { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public ICollection<OrderProduct>? OrderProducts { get; set; }
    }
}
