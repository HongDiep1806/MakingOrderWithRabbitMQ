﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MakingOrder.Models;

namespace MakingOrder.WebModel
{
    public class CustomerResponse
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; } 
        public string PhoneNumber { get; set; }
        public string Address { get; set; } 
        public string UserType { get; set; }
    }
}
