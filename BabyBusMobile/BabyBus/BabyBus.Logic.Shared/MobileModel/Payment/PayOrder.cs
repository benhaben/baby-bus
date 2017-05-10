using System;


namespace BabyBus.Logic.Shared
{
	public class PayOrder
	{

		public string OrderNumber { get; set; }

		public long UserId { get; set; }

		public DateTimeOffset CreateDate { get; set; }

		public PaymentType PaymentType { get; set; }

		public PayOrderStatus Status { get; set; }

		public long ChildId { get; set; }
	}
}

