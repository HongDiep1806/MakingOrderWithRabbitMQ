using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakingOrder.Models
{
    public class Product
    {
        public int ProductId { get; set; } 
        public string ProductName { get; set; } = string.Empty;
        public int Price { get; set; }
        public int StockQuantity { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
    }

}
