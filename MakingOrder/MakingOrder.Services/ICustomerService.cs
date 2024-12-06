using MakingOrder.Models;
using MakingOrder.WebModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MakingOrder.WebModel;

namespace MakingOrder.Services
{
    public interface ICustomerService
    {
        List<Customer> GetAll();
        void Create (CreateCustomerRequest request);
        void PlaceOrder();
        string HashPassword (string password);
        string Login (CustomerLoginRequest request);
        Customer getCustomerByName (string name);
    }
}
