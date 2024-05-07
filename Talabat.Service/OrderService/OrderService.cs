using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Talabat.Core;
using Talabat.Core.Entities.Product;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.OrderSpecs;

namespace Talabat.Application.OrderService
{
	public class OrderService : IOrderService
	{
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Product> _productsRepo;
		private readonly IBasketRepository _basketRepo;
		private readonly IGenericRepository<DeliveryMethod> _deliveryMethodsRepo;
		private readonly IGenericRepository<Order> _orderRepo;

		public OrderService(
             IUnitOfWork unitOfWork,
             IGenericRepository<Product> productsRepo,
			 IBasketRepository basketRepo,
			 IGenericRepository<DeliveryMethod> deliveryMethodsRepo,
			 IGenericRepository<Order> orderRepo
			)
		{
            _unitOfWork = unitOfWork;
            _productsRepo = productsRepo;
			_basketRepo = basketRepo;
			_deliveryMethodsRepo = deliveryMethodsRepo;
			_orderRepo = orderRepo;
		}
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, ShippingAddress shippingAddress)
        {
            // 1.Get Basket From Baskets Repo
            var basket = await _basketRepo.GetBasketAsync(basketId);

			// 2. Get Selected Items at Basket From Products Repo
			List<OrderItem> orderItems = new List<OrderItem>();

			if (basket?.Items?.Count > 0)
			{
				foreach (var item in basket.Items)
				{
                    var product = await _unitOfWork.Repository<Product>().GetAsync(item.Id);
                    var productItemOrdered = new ProductItemOrdered(item.Id, product?.Name ?? String.Empty, product?.PictureUrl ?? String.Empty);

					var orderItem = new OrderItem(productItemOrdered, item.Quantity, product.Price);
					orderItems.Add(orderItem);
				}
			}

			// 3. Calculate SubTotal
			decimal subTotal = orderItems.Sum(item => item.Quantity * item.Price);

            // 4. Get Delivery Method From DeliveryMethods Repo
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAsync(deliveryMethodId);

            // 5. Create Order
            Order order = new Order()
			{
				BuyerEmail = buyerEmail,
				DeliveyMethod = deliveryMethod,
				ShippingAddress = shippingAddress,
				SubTotal = subTotal,
				Items = orderItems,
			};
            _unitOfWork.Repository<Order>().Add(order);


            // 6. Save To Database [TODO]
            var result = await _unitOfWork.Compelete();

            if (result <= 0) 
				return null;
			return order;
		}

        public Task<IReadOnlyList<DeliveryMethod>> GetDelivreyMethodsAsync()
        {
            throw new NotImplementedException();
		}

        public async Task<Order?> GetOrderByIdForUserAsync(string buyerEmail, int orderId)
        {
            var ordersRepos = _unitOfWork.Repository<Order>();
            var orderSpecifications = new OrderSpecifications(buyerEmail, orderId);
            var order = await ordersRepos.GetWithSpecAsync(orderSpecifications);
            return order;
        }

        public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var ordersRepos = _unitOfWork.Repository<Order>();
            var orderSpecifications = new OrderSpecifications(buyerEmail);
			var orders = ordersRepos.GetAllWithSpecAsync(orderSpecifications);
            return orders;
        }
    }
}
