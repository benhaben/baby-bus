using Cirrious.CrossCore;
using System.Collections.Generic;
using System;
using BabyBus.Logic.Shared;

namespace BabyBus.Logic.Shared
{
	public class SettingPaymentViewModel : BaseViewModel
	{
		readonly IRemoteService _service;
		ALPaymentConfig paymentconfig = new ALPaymentConfig();

		float _fee;

		public float Fee {
			get {
				return _fee;
			}
			set {
				_fee = value;
				RaisePropertyChanged(() => DisplayFeePay);
			}
		}

		AddedValue _addedValue;

		public AddedValue AddedValue {
			get { 
				return _addedValue;
			}
			set { 
				_addedValue = value;

			}
		}

		public string DisplayFeePay {
			get {
				var result = AddedValue.CurrentPrice.ToString("C") + "   确认支付";
				return result;
			}
		}

		private IDictionary<string,string> _resultUnifiedOrder;

		public IDictionary<string, string> ResultUnifiedOrder {
			get {
				return _resultUnifiedOrder;
			}
			set {
				_resultUnifiedOrder = value;
			}
		}

		private string _orderNumber;

		public string OrderNumber {
			get {
				return _orderNumber;
			}
			set {
				_orderNumber = value;
			}
		}

		bool paymentStatus;

		public SettingPaymentViewModel()
		{
			_service = Mvx.Resolve<IRemoteService>();
		}

		public override void InitData()
		{
			base.InitData();

			ViewModelStatus = new ViewModelStatus("正在加载服务信息...", true, MessageType.Information, TipsType.DialogProgress);
			AddedValue = _service.GetAddedValue(BabyBusContext.KindergartenId).Result;

			if (AddedValue.Id == 0) {
				ViewModelStatus = new ViewModelStatus("加载服务信息失败", false, MessageType.Error, TipsType.DialogDisappearAuto);
			} else {
				Fee = (float)AddedValue.CurrentPrice;
			}

			try {
				paymentStatus = _service.GetPaymentStatus(PaymentType.ValueAdded).Result;

				ViewModelStatus = new ViewModelStatus("加载订单信息", false, MessageType.Success, TipsType.DialogDisappearAuto);
			} catch (Exception ex) {
				ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Success, TipsType.DialogDisappearAuto);
			}

			if (paymentStatus) {
				ViewModelStatus = new ViewModelStatus("您已经购买该服务，请放心使用", false, MessageType.Success, TipsType.DialogDisappearAuto);
				return;
			}
		}

		bool IsPaid()
		{
			if (paymentStatus) {
				ViewModelStatus = new ViewModelStatus("您已经购买该服务，无需重复购买", false, MessageType.Success, TipsType.DialogDisappearAuto);
				return true;
			}

			ViewModelStatus = new ViewModelStatus("正在转入支付页面...", true, MessageType.Information, TipsType.DialogProgress);
			return false;
		}

		/// <summary>
		/// Payments the processing, payMethodCode: 支付方式代码(1-支付宝；2-微信支付)
		/// </summary>
		/// <param name="payMethodCode">支付方式代码：1-支付宝；2-微信支付</param>
		/// <param name="action">Action.</param>
		public async void PaymentProcessing(int payMethodCode, Action<IDictionary<string,string>> action)
		{
			if (IsPaid()) {
				return;
			}

			var paymentService = new PaymentService();
			if (payMethodCode == 1) {
				OrderNumber = await paymentService.AliPaySendRequest(PaymentType.ValueAdded, 0, Fee, action);
			} else {
				OrderNumber = await paymentService.WXPaySendRequest(PaymentType.ValueAdded, 0, Fee, action);
			}
			if (string.IsNullOrEmpty(OrderNumber)) {
				ViewModelStatus = new ViewModelStatus("生成支付订单失败", false, MessageType.Error,
					TipsType.DialogDisappearAuto);
			} else {
				ViewModelStatus = new ViewModelStatus("", false, MessageType.Information, TipsType.Undisplay);
			}
		}

		public async void CheckPaymentResult()
		{
			if (!string.IsNullOrEmpty(OrderNumber)) {
				ViewModelStatus = new ViewModelStatus("检查订单状态...", true, MessageType.Information, TipsType.DialogProgress);

				var order = await _service.GetPayOrder(OrderNumber);
				if (order == null) {
					ViewModelStatus = new ViewModelStatus("无此订单信息", false, MessageType.Error,
						TipsType.DialogDisappearAuto);
					return;
				}
				if (order.Status != PayOrderStatus.Paid) {
					ViewModelStatus = new ViewModelStatus("还未付款", false, MessageType.Error,
						TipsType.DialogDisappearAuto);
					return;
				}
				paymentStatus = true;
				ViewModelStatus = new ViewModelStatus("支付成功，可以使用我们的增值服务", false, MessageType.Success,
					TipsType.DialogDisappearAuto);
			}
		}
	}
}

