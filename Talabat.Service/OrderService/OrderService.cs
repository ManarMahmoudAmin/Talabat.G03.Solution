using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Product;
using Talabat.Core.Entitites.Order_Aggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;

namespace Talabat.Application.OrderService
{
	public class OrderService : IOrderService
	{
		private readonly IGenaricRepository<Product> _productsRepo;
		private readonly IBasketRepository _basketRepo;
		private readonly IGenaricRepository<DeliveryMethod> _deliveryMethodsRepo;
		private readonly IGenaricRepository<Order> _orderRepo;

		public OrderService(
			 IGenaricRepository<Product> productsRepo,
			 IBasketRepository basketRepo,
			 IGenaricRepository<DeliveryMethod> deliveryMethodsRepo,
			 IGenaricRepository<Order> orderRepo
			)
		{
			_productsRepo = productsRepo;
			_basketRepo = basketRepo;
			_deliveryMethodsRepo = deliveryMethodsRepo;
			_orderRepo = orderRepo;
		}
        public async Task<Order> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
		{
			// 1.Get Basket From Baskets Repo
			var basket = await _basketRepo.GetBasketAsync(basketId);

			// 2. Get Selected Items at Basket From Products Repo
			List<OrderItem> orderItems = new List<OrderItem>();

			if (basket?.Items?.Count > 0)
			{
				foreach (var item in basket.Items)
				{
					var product = await _productsRepo.GetAsync(item.Id);
					var productItemOrdered = new ProductItemOrdered(item.Id, product?.Name ?? String.Empty, product?.PictureUrl ?? String.Empty);

					var orderItem = new OrderItem(productItemOrdered, item.Quantity, product.Price);
					orderItems.Add(orderItem);
				}
			}

			// 3. Calculate SubTotal
			decimal subTotal = orderItems.Sum(item => item.Quantity * item.Price);

			// 4. Get Delivery Method From DeliveryMethods Repo
			var deliveryMethod = await _deliveryMethodsRepo.GetAsync(deliveryMethodId);

			// 5. Create Order
			Order order = new Order()
			{
				BuyerEmail = buyerEmail,
				DeliveyMethod = deliveryMethod,
				ShippingAddress = shippingAddress,
				SubTotal = subTotal,
				Items = orderItems,
			};

			// 6. Save To Database [TODO]
			_orderRepo.Add(order);
			return order;
		}

		public Task<IReadOnlyList<DeliveryMethod>> GetDelivreyMethodsAsync()
		{
			throw new NotImplementedException();
		}

		public Task<Order> GetOrderByIdAsync(string buyerEmail, int orderId)
		{
			throw new NotImplementedException();
		}

		public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
		{
			throw new NotImplementedException();
		}
	}
}
