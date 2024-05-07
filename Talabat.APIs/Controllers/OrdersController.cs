using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entitites.Order_Aggregate;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Controllers
{
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<Order?>> CreateOrder(OrderDTO model)
        {
            var address = _mapper.Map<ShippingAddressDTO, ShippingAddress>(model.ShippingAddress);
            var order = await _orderService.CreateOrderAsync(model.BuyerEmail, model.BasketId, model.DeliveryMethodId, address);
            if (order is null) return BadRequest(new ApiResponse(400));
            return Ok(order);
        }
    }
}
