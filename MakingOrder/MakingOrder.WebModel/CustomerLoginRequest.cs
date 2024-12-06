using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakingOrder.WebModel
{
    public class CustomerLoginRequest
    {
        public required string CustomerName { get; set; }
        public required string Password { get; set; }
    }
}
