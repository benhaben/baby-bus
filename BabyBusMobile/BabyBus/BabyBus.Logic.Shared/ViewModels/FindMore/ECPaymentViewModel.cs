using Cirrious.CrossCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System;
using System.Text;
using BabyBus.Logic.Shared;

namespace BabyBus.Logic.Shared
{
	public class ECPaymentViewModel : BaseViewModel
	{
		readonly IRemoteService _service;


		private int postInfoid;

		private float _fee;

		public float Fee {
			get {
				return _fee;
			}
			set {
				_fee = value;
				RaisePropertyChanged(() => DisplayFeePay);
			}
		}

		private string _dispalyTitle;

		public string DisplayTitle {
			get { 
				return _dispalyTitle;
			}
			set {
				_dispalyTitle = value;
				RaisePropertyChanged(() => DisplayTitle);
			}
		}

		string _abstract;

		public string Abstract {
			get { 
				return _abstract;
			}
			set {
				_abstract = value;
				RaisePropertyChanged(() => Abstract);
			}
		}

		public string DisplayFeePay {
			get {
				var result = ((double)_fee).ToString("C");
				return result;
			}
		}

		bool paymentStatus;

		public string OrderNumber {
			get;
			set;
		}

		private ECPostInfo postinfo;

		public ECPostInfo PostInfo {
			get { 
				return postinfo ?? new ECPostInfo();
			}
			set {
				postinfo = value;
			}
		}

		public PaymentType PaymentType {
			get;
			set;
		}

		public ECPaymentViewModel()
		{
			_service = Mvx.Resolve<IRemoteService>();
		}

		public void Init(int id, PaymentType type)
		{
			postInfoid = id;
			PaymentType = type;
		}

		public async override void InitData()
		{
			base.InitData();
			ViewModelStatus = new ViewModelStatus("正在加载服务信息...", true, MessageType.Information, TipsType.DialogProgress);
			try {
				Task task = Task.Factory.StartNew(() => {
					if (PaymentType == PaymentType.FindMore) {
						postinfo = _service.GetECPostInfoById(postInfoid).Result;
					} else {
						postinfo = _service.GetAudioService().Result;
					}
				});
				//init data本身要是同步的，因为firstload是在另一个线程，这里没必要再异步
				task.Wait();

				if (postinfo == null) {
					ViewModelStatus = new ViewModelStatus("服务信息已失效！", false, MessageType.Success, TipsType.DialogDisappearAuto);
				} else {
					Fee = (float)postinfo.CurrentPrice;
					DisplayTitle = postinfo.Title;
					Abstract = postinfo.Abstract;
				}

				ViewModelStatus = new ViewModelStatus("加载服务信息成功", false, MessageType.Success, TipsType.DialogDisappearAuto);
			} catch (Exception ex) {
				ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Success, TipsType.DialogDisappearAuto);
			}

			ViewModelStatus = new ViewModelStatus("正在加载订单信息...", true, MessageType.Information, TipsType.DialogProgress);
			try {
				paymentStatus = await _service.GetPaymentStatus(PaymentType, postInfoid);

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
				OrderNumber = await paymentService.AliPaySendRequest(PaymentType, postInfoid, Fee, action);
			} else {
				OrderNumber = await paymentService.WXPaySendRequest(PaymentType, postInfoid, Fee, action);
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
				} else {
					
					paymentStatus = true;
					ViewModelStatus = new ViewModelStatus("支付成功，可以使用我们的增值服务", false, MessageType.Success,
						TipsType.DialogWithOkButton);
				}

			}
		}

	}
}

