using System;
using BabyBus.Models.Enums;

namespace BabyBus.Models.Payment
{
	public class PayOrder
	{

		public string OrderNumber { get; set; }

		public int UserId { get; set; }

		public int ChildId{ get; set; }

		public DateTimeOffset CreateDate { get; set; }

		public PaymentType PaymentType { get; set; }

		public PayOrderStatus Status { get; set; }
	}
}

