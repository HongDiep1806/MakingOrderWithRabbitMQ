using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakingOrder.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public UserType UserType { get; set; } = UserType.Customer;
        public string Password {  get; set; } = string.Empty;
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }

}
