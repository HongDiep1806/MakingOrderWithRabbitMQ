using MakingOrder.Models;
using MakingOrder.Repositories;
using MakingOrder.WebModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MakingOrder.Repositories;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;



namespace MakingOrder.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepsitory _customerRepository;
        private readonly IConfiguration _configuration;

        public CustomerService(ICustomerRepsitory customerRepsitory, IConfiguration configuration)
        {
            _customerRepository = customerRepsitory;
            _configuration = configuration;
        }

        public void Create(CreateCustomerRequest request)
        {
            _customerRepository.Create(new Customer
            {
                CustomerId = request.CustomerId,
                FullName = request.FullName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                Password= HashPassword(request.Password),
            });
        }

        public List<Customer> GetAll()
        {
            return _customerRepository.GetAll();
        }

        public Customer getCustomerByName(string name)
        {
            var customers = GetAll();
            var findCustomer = customers.FirstOrDefault(c => c.FullName.Equals(name));
            if (findCustomer != null)
            {
                return findCustomer;
            }
            else
            {
                return null;
            }
        }

        public string HashPassword(string password)
        {
            MD5 md5H = MD5.Create();
            byte[] data = md5H.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder sB = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sB.Append(data[i].ToString("x2"));
            }
            return sB.ToString();
        }

        public string Login(CustomerLoginRequest request)
        {
            var customer = getCustomerByName(request.CustomerName);
            if (customer == null)
            {
                return null;
            }
            else
            {
                if(customer.Password.Equals(HashPassword(request.Password)))
                {
                    Console.WriteLine("hashpass "+HashPassword(request.Password));  
                    string token = CreateToken(customer);
                    return token;
                }
                else
                {
                    return null;
                }   
            }
        }

        public void PlaceOrder()
        {
            throw new NotImplementedException();
        }

        private string CreateToken(Customer customer)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, customer.FullName),
                new Claim(ClaimTypes.Role, customer.UserType.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
