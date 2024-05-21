﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Specifications.OrderSpecs
{
    public class OrderWithPaymenyIntentIdSpecs : BaseSpecifications<Order>
    {
        public OrderWithPaymenyIntentIdSpecs(string paymentIntentId) : base(O => O.PaymentIntentId == paymentIntentId)
        {

        }
    }
}
