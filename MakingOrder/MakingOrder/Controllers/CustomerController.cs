using MakingOrder.Services;
using MakingOrder.WebModel;
using Microsoft.AspNetCore.Mvc;

namespace MakingOrder.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var customers = _customerService.GetAll();
            var results = customers.Select(c => new CustomerResponse
            {
                CustomerId = c.CustomerId,
                FullName = c.FullName,
                PhoneNumber = c.PhoneNumber,
                Address = c.Address,
                Email = c.Email,
                UserType = c.UserType.ToString(),
            });
            return Ok(results);

        }

        [HttpPost("create")]
        public IActionResult Create(CreateCustomerRequest request)
        {
            _customerService.Create(request);
            return Ok();
        }

        [HttpPost("login")]
        public IActionResult Login(CustomerLoginRequest request)
        {
            string token = _customerService.Login(request);
            if (token != null)
            {
                return Ok(token);
            }
            else
            {
                return BadRequest("User not found or Wrong password");
            }
        }

        
    }
}
