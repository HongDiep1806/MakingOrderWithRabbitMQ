using MakingOrder.WebModel;
using MakingOrder.Models;
using MakingOrder.Repositories;

namespace MakingOrder.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public void Create(CreateProductRequest request)
        {
            _productRepository.Create(new Product
            {
                ProductId = request.ProductId,
                ProductName = request.ProductName,
                StockQuantity = request.StockQuantity,
                Price = request.Price
            });

        }

        public List<Product> GetAll()
        {
            return _productRepository.GetAll();
        }

        public Product GetProductById(int id)
        {
            return _productRepository.GetByID(id);
        }
    }
}
