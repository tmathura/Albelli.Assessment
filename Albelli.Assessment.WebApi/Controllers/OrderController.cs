using Albelli.Assessment.Core.Ordering.Interfaces;
using Albelli.Assessment.Domain.Models;
using Albelli.Assessment.WebApi.Filters;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Albelli.Assessment.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderBl _orderBl;
        private readonly ILog _logger = LogManager.GetLogger(typeof(OrderController));

        public OrderController(IOrderBl orderBl)
        {
            _orderBl = orderBl;
        }

        [HttpPost]
        [Route("")]
        [Authorize]
        public async Task<decimal> PlaceOrder(OrderRequest orderRequest)
        {
            try
            {
                _logger.Info("Start placing order process.");

                var order = new Order
                {
                    Id = orderRequest.Id,
                    Products = orderRequest.Products.Select(productRequest => new Product { ProductType = productRequest.ProductType, Quantity = productRequest.Quantity }).ToList()
                };

                var requiredBinWidth = await _orderBl.PlaceOrder(order);

                _logger.Info("Completed placing order process.");

                return requiredBinWidth;
            }
            catch (Exception exception)
            {
                throw new HttpResponseException((int)HttpStatusCode.InternalServerError, exception.Message);
            }
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<OrderDto?> GetOrder(int id)
        {
            try
            {
                _logger.Info("Start getting order process.");

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

                _logger.Info("Completed getting order process.");

                return orderDto;
            }
            catch (Exception exception)
            {
                throw new HttpResponseException((int)HttpStatusCode.InternalServerError, exception.Message);
            }
        }
    }
}