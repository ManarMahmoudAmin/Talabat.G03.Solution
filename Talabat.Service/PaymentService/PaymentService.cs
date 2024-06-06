using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Basket;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Entities.Product;
using Talabat.Core.Repositories.Contract;
using Talabat.Core;
using Stripe;
using Product = Talabat.Core.Entities.Product.Product;
using Talabat.Core.Specifications.OrderSpecs;

namespace Talabat.Application.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configs;
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configs, IBasketRepository basketRepo, IUnitOfWork unitOfWork)
        {
            _configs = configs;
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _configs["StripeSettings:SecurityKey"];

            var basket = await _basketRepo.GetBasketAsync(basketId);
            if (basket is null) return null;
            var shippingPrice = 0m;

            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAsync(basket.DeliveryMethodId.Value);
                shippingPrice = deliveryMethod.Cost;
                basket.ShippingPrice = shippingPrice;
            }

            if (basket.Items.Count > 0)
            {
                var productRepo = _unitOfWork.Repository<Product>();
                foreach (var item in basket.Items)
                {
                    var product = await productRepo.GetAsync(item.Id);
                    if (item.Price != product.Price)
                        item.Price = product.Price;
                }
            }


            PaymentIntent paymentIntent;
            PaymentIntentService paymentIntentService = new PaymentIntentService();
            if (string.IsNullOrEmpty(basket.PaymenyIntentId)) // create
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Quantity * item.Price * 100) + (long)shippingPrice * 100,
                    Currency = "USD",
                    PaymentMethodTypes = new List<string>() { "card" }
                };
                paymentIntent = await paymentIntentService.CreateAsync(options);
                basket.PaymenyIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else // update
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Quantity * item.Price * 100) + (long)shippingPrice * 100,
                };
                paymentIntent = await paymentIntentService.UpdateAsync(basket.PaymenyIntentId, options);
            }
            await _basketRepo.UpdateBasketAsync(basket);
            return basket;
        }

        public async Task<Order?> UpdateOrderStatus(string paymentIntentId, bool isPaid)
        {
            var orderSpec = new OrderWithPaymenyIntentIdSpecs(paymentIntentId);
            var exsitOrder = await _unitOfWork.Repository<Order>().GetWithSpecAsync(orderSpec);
            if (exsitOrder is null) return null;

            if (isPaid)
                exsitOrder.Status = OrderStatus.PaymentRecived;
            else
                exsitOrder.Status = OrderStatus.PaymenyFailed;

            _unitOfWork.Repository<Order>().Update(exsitOrder);
            await _unitOfWork.Compelete();
            return exsitOrder;
        }
    }
}
