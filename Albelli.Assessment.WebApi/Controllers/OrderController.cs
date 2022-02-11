using Albelli.Assessment.Core.Ordering.Interfaces;
using Albelli.Assessment.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Albelli.Assessment.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderBl _orderBl;

        public OrderController(IOrderBl orderBl)
        {
            _orderBl = orderBl;
        }

        [HttpPost]
        public async Task<decimal> CreateEmployee(OrderRequest orderRequest)
        {
            var order = new Order
            {
                Id = orderRequest.Id,
                Products = orderRequest.Products.Select(productRequest => new Product { ProductType = productRequest.ProductType, Quantity = productRequest.Quantity }).ToList()
            };

            return await _orderBl.PlaceOrder(order);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<OrderDto> GetOrder(int id)
        {
            var order = await _orderBl.GetOrder(id);
            var orderDto = new OrderDto
            {
                Id = order.Id,
                Products = order.Products.Select(productRequest => new ProductDto { ProductType = productRequest.ProductType, Quantity = productRequest.Quantity }).ToList(),
                RequiredBinWidth = order.RequiredBinWidth
            };

            return orderDto;
        }
    }
}