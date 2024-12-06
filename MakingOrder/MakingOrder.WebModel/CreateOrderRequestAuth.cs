using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakingOrder.WebModel
{
    public class CreateOrderRequestAuth
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        //public int TotalAmount { get; set; }
        public int CustomerId { get; set; }
        public string Email { get; set; }   
        public List<OrderProductRequest> Products { get; set; } = new List<OrderProductRequest>();

    }
}
