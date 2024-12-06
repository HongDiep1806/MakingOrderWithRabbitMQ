using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MakingOrder.Models;
using MakingOrder.WebModel;

namespace MakingOrder.Services
{
    public interface IProductService
    {
        List<Product> GetAll();
        void Create(CreateProductRequest request);
        Product GetProductById(int id); 

    }
}
