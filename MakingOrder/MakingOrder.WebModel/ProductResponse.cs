namespace MakingOrder.WebModel
{
    public class ProductResponse
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Price { get; set; }
        public int StockQuantity { get; set; }

    }
}
