using Albelli.Assessment.Core.Ordering.Interfaces;
using Albelli.Assessment.Domain.Models;
using Albelli.Assessment.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        [Route("")]
        public async Task<decimal> CreateOrder(OrderRequest orderRequest)
        {
            try
            {
                var order = new Order
                {
                    Id = orderRequest.Id,
                    Products = orderRequest.Products.Select(productRequest => new Product { ProductType = productRequest.ProductType, Quantity = productRequest.Quantity }).ToList()
                };

                return await _orderBl.PlaceOrder(order);
            }
            catch (Exception exception)
            {
                throw new HttpResponseException((int)HttpStatusCode.InternalServerError, exception.Message);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<OrderDto?> GetOrder(int id)
        {
            try
            {
                OrderDto? orderDto = null;
                var order = await _orderBl.GetOrder(id);

                if (order != null)
                {
                    orderDto = new OrderDto
                    {
                        Id = order.Id,
                        Products = order.Products.Select(productRequest => new ProductDto { ProductType = productRequest.ProductType, Quantity = productRequest.Quantity }).ToList(),
                        RequiredBinWidth = order.RequiredBinWidth
                    };
                }

                return orderDto;
            }
            catch (Exception exception)
            {
                throw new HttpResponseException((int)HttpStatusCode.InternalServerError, exception.Message);
            }
        }
    }
}