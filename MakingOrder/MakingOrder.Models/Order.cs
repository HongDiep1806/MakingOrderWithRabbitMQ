namespace MakingOrder.Models
{
    public class Order
    {
        public int OrderId { get; set; } 
        public DateTime OrderDate { get; set; }
        public int TotalAmount { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;
        public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
    }

}
