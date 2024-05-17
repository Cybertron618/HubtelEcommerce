namespace HubtelEcommerce.Api.Models
{
    public class Order
    {
        public int ID { get; set; }

        public int CustomerID { get; set; } // Changed to int to match the Customer ID type

        public DateTime OrderDate { get; set; }

        public decimal Total { get; set; } // Fixed the type definition

        public ICollection<OrderProduct>? OrderProducts { get; set; }
    }
}
