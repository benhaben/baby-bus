using System;
using System.Threading.Tasks;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using Xamarin;
using BabyBusSSApi.ServiceModel.DTO.Create.Report;


namespace BabyBus.Logic.Shared
{
	public class BaseViewModel : MvxViewModel
	{
		private IRemoteService _remoteService = null;

		public event EventHandler FirstLoadedEventHandler;

		public BaseViewModel()
		{
			PageIndex = 0;
			PageSize = Constants.PAGESIZE;
			IsFirst = true;
			_remoteService = Mvx.Resolve<IRemoteService>();
		}

		public static AppType AppType { get; set; }

		public int VerCode { get; set; }

		public string VerName { get; set; }
		//Mobile Phone Model
		public string PhoneMode{ get; set; }

		/// <summary>
		/// OSType.Value is Android or iOS.
		/// </summary>
		public string OSType{ get; set; }

		public static BaseViewModel CurrentBaseViewModel { get; private set; }

		private ViewModelStatus _viewModelStatus = new ViewModelStatus("");

		public ViewModelStatus ViewModelStatus {
			get { return _viewModelStatus; }
			set {
				_viewModelStatus = value;
				if (InfoChanged != null) {
					InfoChanged(this, null);
				}
				RaisePropertyChanged(() => ViewModelStatus);
			}
		}

		public virtual void InitData()
		{
		}

		public virtual void FirstLoad()
		{
			Task.Factory.StartNew(() => {
				try {
					InitData();
					FirstLoadEventInvoke();
				} catch (Exception ex) {
					Insights.Report(ex, Insights.Severity.Error);
				}
			});
			//
//            task.Wait();
		}

		public virtual void FirstLoadEventInvoke()
		{
			if (FirstLoadedEventHandler != null) {
				try {
					this.InvokeOnMainThread(() => FirstLoadedEventHandler(this, null));
				} catch (Exception ex) {
					Insights.Report(ex, Insights.Severity.Error);
				}
			}
		}

		//Enter Log Stats
		public void EnterLog()
		{
			var report = new CreateUserReport {
				VerCode = VerCode,
				VerName = VerName,
				OSType = OSType,
				Mode = PhoneMode,
			};

			Task.Run(() => {
				_remoteService.CreateReport(report);	
			});
		}

		public bool IsSuccessStatus {
			get { return ViewModelStatus != null && ViewModelStatus.MessageType == MessageType.Success; }

		}

		public bool IsErrorStatus {
			get { return ViewModelStatus != null && ViewModelStatus.MessageType == MessageType.Error; }

		}

		public bool IsInformationStatus {
			get { return ViewModelStatus != null && ViewModelStatus.MessageType == MessageType.Information; }

		}

		public bool IsWarningStatus {
			get { return ViewModelStatus.MessageType == MessageType.Warning; }

		}

		public bool IsRunning {
			get { return ViewModelStatus.IsRunning; }

		}

		public string Information {
			get { return ViewModelStatus.Information; }
			set { ViewModelStatus.Information = value; }
		}

		public event EventHandler InfoChanged;

		public int PageIndex { get; set; }

		public int PageSize { get; set; }

		public bool IsFirst { get; set; }
	}

}
