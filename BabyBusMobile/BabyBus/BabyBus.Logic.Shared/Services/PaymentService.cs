using System;
using System.Collections.Generic;
using Cirrious.CrossCore;
using System.Threading.Tasks;
using System.Text;

namespace BabyBus.Logic.Shared
{
	public class PaymentService : IPaymentService
	{

		readonly IRemoteService _service;
		ALPaymentConfig _alPaymentConfig = new ALPaymentConfig();
		WXPaymentConfig _wxPaymentConfig = new WXPaymentConfig();



		public PaymentService()
		{
			_service = Mvx.Resolve<IRemoteService>();
		}

		#region IPaymentService implementation

		public async Task<string> AliPaySendRequest(PaymentType type, int postInfoId, float fee, Action<IDictionary<string,string>> action)
		{
			//1. Gen PayOrder OrderNumber
			var orderNumber = await GenPayOrderNumber(postInfoId, type);
			if (string.IsNullOrEmpty(orderNumber)) {
				return string.Empty;
			}

			//2. Gen Pre PayInfo
			var requestParams = new Dictionary<string,string>();
			var payMany = Math.Round(fee, 2).ToString();
			requestParams.Add("alpayinfo", _alPaymentConfig.getOrderInfo("陕西睿莱智能科技有限公司", "增值服务", payMany, orderNumber));

			//3. Send Pay Request
			action.Invoke(requestParams);

			return orderNumber;
		}

		public async Task<string> WXPaySendRequest(PaymentType type, int postInfoId, float fee, Action<IDictionary<string, string>> action)
		{
			//1. Gen PayOrder OrderNumber
			var orderNumber = await GenPayOrderNumber(postInfoId, type);
			if (string.IsNullOrEmpty(orderNumber)) {
				return string.Empty;
			}

			//2. Gen Pre PayId
			var requestParams = await _wxPaymentConfig.GetOrderInfo(orderNumber, fee);
			if (requestParams == null) {
				return string.Empty;
			}

			//3. Send Pay Request
			action.Invoke(requestParams);

			return orderNumber;
		}

		#endregion



		async Task<string> GenPayOrderNumber(int postinfoId, PaymentType type)
		{
			var orderNo = await _service.GenarateNewOrderNumber(postinfoId, type);
			return orderNo;
		}


	}
}

