using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using BabyBus.Logic.Shared;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;


namespace BabyBus.Logic.Shared
{
	public class UnattenceChildrenViewModel : BaseViewModel
	{
		private readonly IRemoteService service;
		private DateTime _date;

		public UnattenceChildrenViewModel()
		{
			service = Mvx.Resolve<IRemoteService>();
		}

		public void Init(DateTime date) {
			_date = date;
		}

       
		void InitChildrenList() {
			ViewModelStatus = new ViewModelStatus("正在加载...", true, MessageType.Information, TipsType.DialogProgress);
           
			var task = Task.Factory.StartNew(async () => {
				UserModel user = BabyBusContext.UserAllInfo;
				try {
					ApiResult<ChildModel> result = await service.GetAttendanceChildList(BabyBusContext.ClassId, _date, AttenceType.UnAttence);
					if (!result.Status) {
						ViewModelStatus = new ViewModelStatus(result.Message);
					} else {
						Children = new List<ChildModel>(result.Items);
					}
				} catch (Exception ex) {
					Debug.WriteLine(ex.Message);
					ViewModelStatus = new ViewModelStatus("加载新数据失败", false, MessageType.Error, TipsType.DialogDisappearAuto);
				} finally {
				}

				ViewModelStatus = new ViewModelStatus("加载成功", false, MessageType.Success, TipsType.Undisplay);
			});
			task.Wait();
		}

		public override void InitData() {
			InitChildrenList();
           
		}

		



		#region Property

		
		
		private IList<ChildModel> children = new List<ChildModel>();

		/// <summary>
		///     Selected Checkout Object, Convert To Json for Detail Page
		/// </summary>
		public string SelectedCheckoutJson { get; set; }

		public IList<ChildModel> Children {
			get { return children; }
			set {
				children = value;
				RaisePropertyChanged(() => Children);
			}
		}

		



		#endregion

		
		
		



		#region Command

		
		
		private IMvxCommand showDetailCommand;

		public IMvxCommand ShowDetailCommand {
			get {
				showDetailCommand = showDetailCommand ??
				new MvxCommand(
					() => ShowViewModel<ChildInfoViewModel>(
						new {json = SelectedCheckoutJson}));
				return showDetailCommand;
			}
		}

		



		#endregion
	}
}