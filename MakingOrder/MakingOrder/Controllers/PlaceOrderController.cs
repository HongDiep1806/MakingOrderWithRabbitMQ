using MakingOrder.Models;
using MakingOrder.Services;
using MakingOrder.WebModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MakingOrder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Customer")]

    public class PlaceOrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;

        public PlaceOrderController(IOrderService orderService,ICustomerService customerService)
        {
            _orderService = orderService;
            _customerService = customerService;
        }

        [HttpGet("getAll")]
        [Authorize(Roles ="Admin")]
        public IActionResult Get()
        {
            var orders = _orderService.GetAll();
            var results = orders.Select(x => new OrderResponse
            {
                CustomerId = x.CustomerId,
                OrderId = x.OrderId,
                OrderDate = x.OrderDate,
                TotalAmount = x.TotalAmount
            });
            return Ok(results);

        }

        [HttpGet("getYourOrders")]
        public IActionResult GetMyOrders()
        {
            var customerName = User.FindFirstValue(ClaimTypes.Name);
            var customer = _customerService.getCustomerByName(customerName);
            var orders = _orderService.GetYourOrders(customer.CustomerId);
            var results = orders.Select(o => new OrderResponse
            {
                CustomerId = o.CustomerId,
                OrderId = o.OrderId,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount
            });
            return Ok(results);
        }

        [HttpPost]
        public IActionResult Create(CreateOrderRequestCheckAuth request)
        {
            var customerName = User.FindFirstValue(ClaimTypes.Name);
            var customer = _customerService.getCustomerByName(customerName);
            CreateOrderRequestAuth rq = new CreateOrderRequestAuth
            {
                CustomerId = customer.CustomerId,
                OrderDate = request.OrderDate,
                OrderId = request.OrderId,
                Products = request.Products,
                Email = customer.Email,
            };

            _orderService.Create(rq);
            return Ok();
        }

    }
}
