using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Basket;

namespace Talabat.Core.Entities.Basket
{
    public class CustomerBasket
    {
        public string Id { get; set; } = null!;
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();
        public CustomerBasket(string id)
        {
            Id = id;
        }

        public string? PaymenyIntentId { get; set; }
        public string? ClintSecret { get; set; }

        public decimal? ShippingPrice { get; set; }
        public int? DeliveryMethodId { get; set; }
    }
}
