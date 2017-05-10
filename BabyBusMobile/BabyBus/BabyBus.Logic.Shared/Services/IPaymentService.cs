using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BabyBus.Logic.Shared
{
	public interface IPaymentService
	{
		/// <summary>
		/// AliPay send request.
		/// </summary>
		/// <returns>The OrderNumber</returns>
		Task<string> AliPaySendRequest(PaymentType type, int postInfoId, float fee, Action<IDictionary<string,string>> action);

		/// <summary>
		/// WXPay send request
		/// </summary>
		/// <returns>The OrderNumber</returns>
		Task<string> WXPaySendRequest(PaymentType type, int postInfoId, float fee, Action<IDictionary<string,string>> action);
	}
}

