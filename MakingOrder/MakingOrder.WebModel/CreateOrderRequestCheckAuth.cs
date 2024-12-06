
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakingOrder.WebModel
{
    public class CreateOrderRequestCheckAuth
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderProductRequest> Products { get; set; } = new List<OrderProductRequest>();
    }
}
