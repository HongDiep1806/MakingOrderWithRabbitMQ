using MakingOrder.Models;
using MakingOrder.Repositories;
using MakingOrder.Services;
using MakingOrder.WebModel;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MakingOrder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public ActionResult GetAllProducts()
        {
            var products = _productService.GetAll();
            var results = products.Select(p => new ProductResponse 
                                                   { ProductId = p.ProductId,
                                                     ProductName=p.ProductName,
                                                     Price=p.Price,StockQuantity=p.StockQuantity})
                                                    .ToList();
            return Ok(results);
        }

        //[HttpGet("{id}")]
        //public ActionResult<Product> GetProductById(string id)
        //{
        //    var product = _productRepository.GetByID(id);
        //    if (product == null)
        //    {
        //        return NotFound(); 
        //    }
        //    return Ok(product); 
        //}

        [HttpPost]
        public ActionResult CreateProduct(CreateProductRequest request)
        {
            _productService.Create(request);
            return Ok(); 
        }

        //[HttpPut("{id}")]
        //public ActionResult UpdateProduct(string id, Product product)
        //{
        //    var existingProduct = _productRepository.GetByID(id);
        //    if (existingProduct == null)
        //    {
        //        return NotFound(); 
        //    }

        //    _productRepository.Update(id, product);
        //    return NoContent(); 
        //}

        //[HttpDelete("{id}")]
        //public ActionResult DeleteProduct(string id)
        //{
        //    var existingProduct = _productRepository.GetByID(id);
        //    if (existingProduct == null)
        //    {
        //        return NotFound();
        //    }

        //    _productRepository.Delete(id);
        //    return NoContent(); 
        //}
    }
}
