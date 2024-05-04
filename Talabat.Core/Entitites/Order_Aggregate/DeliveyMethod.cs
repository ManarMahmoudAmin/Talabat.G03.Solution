﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Entitites.Order_Aggregate
{
	public class DeliveyMethod : BaseEntity
	{
		public string ShortName { get; set; } = null!;
		public string Description { get; set; } = null!;
		public decimal Cost { get; set; }
		public string DeliveyTime { get; set; } = null!;
	}
}
