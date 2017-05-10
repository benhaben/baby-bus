using System;
using System.Collections.Generic;
using System.Linq;


using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using BabyBus.Logic.Shared;
using BabyBusSSApi.ServiceModel.Enumeration;




namespace BabyBus.Logic.Shared
{
	public class AttendanceMasterViewModel : BaseViewModel
	{
		private IRemoteService _service;

		public DateTime CurrentDate {
			get{ return DateTime.Now.Date; }
		}

		public DateTime Date {
			get;
			set;
		}

		public AttendanceMasterViewModel()
		{
			_service = Mvx.Resolve<IRemoteService>();
		}

		public void Init()
		{
			Date = CurrentDate;
		}

		void InitMasterData()
		{
			ViewModelStatus = new ViewModelStatus(UIConstants.LOADING, true, MessageType.Information, TipsType.DialogProgress);
			try {
				Attendances = _service.GetAttendanceMasterList(Date).Result;
				ViewModelStatus = new ViewModelStatus(UIConstants.LOAD_SUCCESS, false, MessageType.Success, TipsType.Undisplay);

				if (Attendances.Count > 0) {
					var model = Attendances[0];
					MasterId = model.MasterId;
					ClassId = BabyBusContext.Class.ClassId;
					ClassName = BabyBusContext.Class.ClassName;
					Total = model.Total;
					Attence = model.Attence;
					IsAttence = true;

					TotalTotal = Attendances.Sum(x => x.Total);
					TotalAttence = Attendances.Where(x => x.IsAttence).Sum(x => x.Attence);
					TotalAbsence = Attendances.Where(x => x.IsAttence).Sum(x => x.UnAttence);
					TotalUnattence = Attendances.Where(x => !x.IsAttence).Sum(x => x.Total);
				} else {
					MasterId = 0;
					ClassId = BabyBusContext.Class.ClassId;
					ClassName = BabyBusContext.Class.ClassName;
					Total = 0;
					Attence = 0;
					IsAttence = false;
					TotalTotal = 0;
					TotalAttence = 0;
					TotalAbsence = 0;
					TotalUnattence = 0;

				}
				if (BabyBusContext.RoleType == RoleType.Teacher) {
					InitChildrenList(IsAttence);
				}
			} catch (Exception ex) {
				Mvx.Trace(ex.Message);
				ViewModelStatus = new ViewModelStatus("加载考勤数据失败，请检查网络", false, MessageType.Error, TipsType.DialogDisappearAuto);
			} finally {
			}
              
		}

		void InitChildrenList(bool isAttence)
		{
			ViewModelStatus = new ViewModelStatus(UIConstants.LOADING, true, MessageType.Information, TipsType.DialogProgress);
           
			try {
				if (isAttence) {
					Children = (from q in _service.GetChildrenAttendance(Date).Result
					            where !q.IsSelect
					            orderby q.IsAskForLeave
					            select q).ToList();
				} else {
					Children = (from q in _service.GetChildrenAttendance(Date).Result
					            where q.IsAskForLeave
					            select q).ToList();
				}

				ViewModelStatus = new ViewModelStatus(UIConstants.LOAD_SUCCESS, false, MessageType.Success, TipsType.Undisplay);
			} catch (Exception ex) {
				ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Error, TipsType.DialogDisappearAuto);
			} finally {
			}
              

		}


		public  override void InitData()
		{
			InitMasterData();
           
		}

		#region Property

		private IList<ChildModel> _children = new List<ChildModel>();

		/// <summary>
		///     Selected Checkout Object, Convert To Json for Detail Page
		/// </summary>
		public string SelectedCheckoutJson { get; set; }

		public IList<ChildModel> Children {
			get { return _children; }
			set {
				_children = value;
				RaisePropertyChanged(() => Children);
			}
		}


		private long _attence;

		public long MasterId { get; set; }

		public long ClassId { get; set; }

		string _className;

		public string ClassName {
			get{ return _className; }
			set { 
				_className = value;
				RaisePropertyChanged(() => ClassName);
			} 
		}

		public long Total { get; set; }

		private long _totalTotal;

		public long TotalTotal {
			get { return _totalTotal; }
			set {
				_totalTotal = value;
				RaisePropertyChanged(() => TotalTotal);
			}
		}

		private long _totalAttence;

		public long TotalAttence {
			get { return _totalAttence; }
			set {
				_totalAttence = value;
				RaisePropertyChanged(() => TotalAttence);
			}
		}

		private long _toalUnattence;

		/// <summary>
		/// 未考勤人数
		/// </summary>
		public long TotalUnattence {
			get { return _toalUnattence; }
			set {
				_toalUnattence = value;
				RaisePropertyChanged(() => TotalUnattence);
			}
		}

		private long _totalAbsence;

		/// <summary>
		/// 缺席总人数
		/// </summary>
		public long TotalAbsence {
			get { return _totalAbsence; }
			set {
				_totalAbsence = value;
				RaisePropertyChanged(() => TotalAbsence);
			}
		}

		public long Attence {
			get { return _attence; }
			set {
				_attence = value;
				RaisePropertyChanged(() => Attence);
				RaisePropertyChanged(() => UnAttence);
			}
		}

		public long UnAttence {
			get { return Total - Attence; }
		}

		private bool _isAttence;

		public bool IsAttence {
			get { return _isAttence; }
			private set { 
				var datestr = Date.Date == CurrentDate ?
					"今日" : Date.ToString("M");
				_isAttence = value;
				AttenceHint = _isAttence ?
					"您可以点击上方的按钮对已结束的考勤进行修改" :
					"本日还未进行考勤，请点击上方的按钮开始考勤";
				ChildListHint = _isAttence ?
					datestr + "缺勤幼儿" :
					datestr + "请假幼儿";
				AttendanceDate = Date.ToString("D") + "考勤详情";
				RaisePropertyChanged(() => AttenceHint);
				RaisePropertyChanged(() => ChildListHint);
				RaisePropertyChanged(() => AttendanceDate);

			}
		}

		public string AttenceHint{ get; private set; }

		public string AttendanceDate{ get; private set; }

		public string ChildListHint{ get; private set; }

		private List<AttendanceMasterModel> attendances = new List<AttendanceMasterModel>();

		public List<AttendanceMasterModel> Attendances {
			get { return attendances; }
			set {
				attendances = value; 
				RaisePropertyChanged(() => Attendances);
			}
		}

		#endregion

		#region Command

		private MvxCommand _showDetailCommand;

		private MvxCommand _showUnAttenceChildrenCommand;

		public MvxCommand ShowDetailCommand {
			get {
				_showDetailCommand = _showDetailCommand ??
				new MvxCommand(
					() =>
						ShowViewModel<AttendanceDetailViewModel>(
						new {masterId = MasterId, classId = ClassId, date = Date}));
				return _showDetailCommand;
			}
		}

		//        public MvxCommand ShowUnAttenceChildrenCommand
		//        {
		//            get
		//            {
		//                _showUnAttenceChildrenCommand = _showUnAttenceChildrenCommand ??
		//                new MvxCommand(
		//                    () =>
		//						ShowViewModel<UnattenceChildrenViewModel>(
		//                        new {date = Date}));
		//                return _showUnAttenceChildrenCommand;
		//            }
		//        }

		#endregion
	}
}