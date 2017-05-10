using System;
using System.Collections.Generic;
using System.Linq;
using BabyBus.Logic.Shared;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using BabyBusSSApi.ServiceModel.DTO.Create;

namespace BabyBus.Logic.Shared
{
	public class AttendanceDetailViewModel : BaseViewModel
	{
		private DateTime _date;

		/// <summary>
		/// a flag to wait the init data loading
		/// </summary>
		private bool loadendflg = false;

		public DateTime Date {
			get {
				return _date;
			}
			set {
				_date = value;
			}
		}

		private int _masterId;
		private int _classId;
		private readonly IRemoteService _service;

		public AttendanceDetailViewModel()
		{
			_service = Mvx.Resolve<IRemoteService>();
			#if DEBUG1
            Children.Add(new ChildModel("yin"));
            Children.Add(new ChildModel("shen"));
            Children.Add(new ChildModel("shen1"));
            Children.Add(new ChildModel("shen2"));
            Children.Add(new ChildModel("shen3"));
            Children.Add(new ChildModel("shen4"));
            Children.Add(new ChildModel("shen5"));
            Children.Add(new ChildModel("shen6"));
            Children.Add(new ChildModel("shen7"));
            Children.Add(new ChildModel("shen8"));
            Children.Add(new ChildModel("shen8"));
            Children.Add(new ChildModel("shen8"));
            Children.Add(new ChildModel("shen8"));
            Children.Add(new ChildModel("shen8"));
            Children.Add(new ChildModel("shen8"));
            Children.Add(new ChildModel("shen8"));
            Children.Add(new ChildModel("shen8"));
            Children.Add(new ChildModel("shen8"));
            Children.Add(new ChildModel("shen8"));
            Children.Add(new ChildModel("shen8"));
            Children.Add(new ChildModel("shen8"));
            Children.Add(new ChildModel("shen8"));
            Children.Add(new ChildModel("shen8"));
            Children.Add(new ChildModel("shen8"));
            Children.Add(new ChildModel("shen8"));
            Children.Add(new ChildModel("shen8"));


			#endif
		}

		public void Init(int masterId, int classId, DateTime date)
		{
			_masterId = masterId;
			_classId = classId;
			_date = date;
		}

		public override  void InitData()
		{
			attendane();
           
			try {
				ViewModelStatus = new ViewModelStatus(UIConstants.LOADING, true, MessageType.Information, TipsType.DialogProgress);
				Children = (from q in _service.GetChildrenAttendance(Date).Result
				            orderby q.IsSelect
				            orderby q.IsAskForLeave
				            select q).ToList();

				ViewModelStatus = new ViewModelStatus(UIConstants.LOAD_SUCCESS, false, MessageType.Success, TipsType.Undisplay);
			} catch (Exception ex) {
				ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Error, TipsType.DialogDisappearAuto);
			}
             
			loadendflg = true;
		}

		#region Property

		private List<ChildModel> children = new List<ChildModel>();

		public List<ChildModel> Children {
			get { return children; }
			set {
				children = value;
			}
		}

		public int Total {
			get { return Children.Count; }
		}

		public int Attence {
			get { return Children.Count(x => x.IsSelect); }
		}

		public void attendane()
		{
			AttenDate = _date.ToString("D") + "考勤详情";
			RaisePropertyChanged(() => AttenDate);
		}

		public string AttenDate{ get; private set; }

		#endregion

		#region Command

		private MvxCommand _attenceCommand;

		public MvxCommand AttenceCommand {
			get {
				_attenceCommand = _attenceCommand ?? new MvxCommand(async() => {
					var childrenPresenceList = (from child in Children
					                            where child.IsSelect
					                            select child.ChildId).ToList();

					var childrenAbsenceList = (from child in Children
					                           where !child.IsSelect
					                           select child.ChildId).ToList();



					var model = new CreateAttendance {
						MasterId = _masterId,
						Total = Children.Count,
						Attence = childrenPresenceList.Count,
						ChildrenPresenceList = childrenPresenceList,
						ChildrenAbsenceList = childrenAbsenceList,
						CreateDate = new DateTimeOffset(Date.Date)
					};
					//if(loadresult.Status){
					if (loadendflg) {  
						try {
							ViewModelStatus = new ViewModelStatus("考勤中...", true);
							await _service.WorkAttendance(model);
							ViewModelStatus = new ViewModelStatus("考勤已经完成!", false, MessageType.Success, TipsType.DialogDisappearAuto);
							Close(this);
						} catch (Exception ex) {
							ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Success, TipsType.DialogDisappearAuto);
						}
					} else {
						ViewModelStatus = new ViewModelStatus("正在加载宝宝名单，请稍等……", false, MessageType.Success, TipsType.DialogDisappearAuto);
					}
				});
					
				return _attenceCommand;
			}
		}

		#endregion
	}
}