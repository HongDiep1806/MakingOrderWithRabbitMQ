using DAL;
using MakingOrder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakingOrder.Repositories
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepsitory
    {
        public CustomerRepository(DataContext context): base(context) { }
    }
}
