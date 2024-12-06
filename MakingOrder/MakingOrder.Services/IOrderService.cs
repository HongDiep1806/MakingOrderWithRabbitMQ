using MakingOrder.Models;
using MakingOrder.WebModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MakingOrder.WebModel;

namespace MakingOrder.Services
{
    public interface IOrderService
    {
        List<Order> GetAll();
        List<Order> GetYourOrders (int customerID);
        void Create(CreateOrderRequestAuth request);
        string MakingEmailBody(List<OrderProductRequest> orderedProducts);
        void SendEmail(string htmlContent, string sentEmail);
        
    }
}
