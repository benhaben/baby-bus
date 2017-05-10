using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cirrious.CrossCore;



namespace BabyBus.Logic.Shared
{
	public class ChildAttendanceViewModel : BaseViewModel
	{

		private readonly IRemoteService _service;
		//        不重复项的无序列表
		public HashSet<DateTime> AttenceDates = new HashSet<DateTime>();

		public int AttenceTotal {
			get { 
                
				var query = from dt in AttenceDates
				            where dt.Month == this.TheDateTime.Month && dt.Year == TheDateTime.Year
				            select dt;

				return query.Count(); 
			}
		}

		public ChildAttendanceViewModel()
		{
			_service = Mvx.Resolve<IRemoteService>();

			//Test Init
			#if DEBUG1
            TestInit();
			#endif
		}

		public DateTime TheDateTime = DateTime.Now;

		public DateTime NextMonthDateTime {
			get {
				return TheDateTime.AddMonths(1);
			}
		}

		public DateTime PrevoiusMonthDateTime {
			get {
				return TheDateTime.AddMonths(-1);
			}
		}


		async Task Request(DateTime date)
		{
			var result = await _service.GetChildMonthAttendance(date);
          
			lock (AttenceDates) {
				foreach (var dt in result) {
					AttenceDates.Add(dt.CreateDate.DateTime);
				}
			}

			RaisePropertyChanged(() => AttenceTotal);
		}


		public  void ReloadData()
		{
			ViewModelStatus = new ViewModelStatus("正在加载...", true, MessageType.Information, TipsType.DialogProgress);

			Task.Factory.StartNew(async () => {
           
				try {
					await Request(this.TheDateTime);
					await Request(this.NextMonthDateTime);
					await Request(this.PrevoiusMonthDateTime);
				} catch (Exception ex) {
					Debug.WriteLine(ex.Message);
					ViewModelStatus = new ViewModelStatus("加载失败", false, MessageType.Error, TipsType.DialogDisappearAuto);
					return;
				} finally {
				}
				ViewModelStatus = new ViewModelStatus("加载成功", false, MessageType.Success, TipsType.Undisplay);
			});
		}

		public override void InitData()
		{
			base.InitData();
			ViewModelStatus = new ViewModelStatus("正在加载...", true, MessageType.Information, TipsType.DialogProgress);

			var task = Task.Factory.StartNew(async () => {
				try {
					await Request(this.TheDateTime);
					await Request(this.NextMonthDateTime);
					await Request(this.PrevoiusMonthDateTime);
				} catch (Exception ex) {
					ViewModelStatus = new ViewModelStatus("加载失败", false, MessageType.Error, TipsType.DialogDisappearAuto);
					Debug.WriteLine(ex.Message);
				} finally {
				}
				ViewModelStatus = new ViewModelStatus("加载成功", false, MessageType.Success, TipsType.Undisplay);
			});
			task.Wait();
		}

		private void TestInit()
		{
			AttenceDates.Add(new DateTime(2015, 3, 1));
			AttenceDates.Add(new DateTime(2015, 3, 2));
			AttenceDates.Add(new DateTime(2015, 3, 3));
			AttenceDates.Add(new DateTime(2015, 3, 4)); 
			AttenceDates.Add(new DateTime(2015, 3, 5));

			AttenceDates.Add(new DateTime(2015, 3, 11));
			AttenceDates.Add(new DateTime(2015, 3, 12));
			AttenceDates.Add(new DateTime(2015, 3, 13));
			AttenceDates.Add(new DateTime(2015, 3, 14));
			AttenceDates.Add(new DateTime(2015, 3, 15));

			AttenceDates.Add(new DateTime(2015, 3, 23));
			AttenceDates.Add(new DateTime(2015, 3, 24));

			RaisePropertyChanged(() => AttenceTotal);
		}
	}
}
