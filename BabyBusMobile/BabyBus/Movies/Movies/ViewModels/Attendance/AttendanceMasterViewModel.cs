using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using BabyBus.Models.Attendance;
using BabyBus.Net.Attendance;
using BabyBus.Utilities;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Binding.Combiners;
using Cirrious.MvvmCross.ViewModels;

namespace BabyBus.ViewModels.Attendance {
    public class AttendanceMasterViewModel : BaseViewModel {
        private readonly IAttendanceService service;

        public AttendanceMasterViewModel() {
            service = Mvx.Resolve<IAttendanceService>();
        }

        //        public override void Start() {
        //            base.Start();
        //
        //            FirstLoad();
        //        }

        public  override void FirstLoad() {
            IsLoading = true;
            ViewModelStatus = new ViewModelStatus("正在加载...", true, MessageType.Information, TipsType.DialogProgress);
            if (NumberOfFirstLoadThread > 0) {
                return;
            }
            NumberOfFirstLoadThread += 1;
            Task.Run(async () => {
                try {
                    var list = await service.GetTodayAttendanceMaster(BabyBusContext.ClassId);
                    Attendances = new ObservableCollection<AttendanceMasterModel>(list);
                    InvokeOnMainThread(() => {
                        if (list.Count > 0) {
                            var model = list[0];
                            MasterId = model.MasterId;
                            ClassId = model.ClassId;
                            ClassName = BabyBusContext.Class.ClassName;
                            Total = model.Total;
                            Attence = model.Attence;
                            IsAttence = model.MasterId != 0;

                            TotalTotal = Attendances.Sum(x => x.Total);
                            TotalAttence = Attendances.Where(x => x.IsAttence).Sum(x => x.Attence);
                            TotalAbsence = Attendances.Where(x => x.IsAttence).Sum(x => x.UnAttence);
                            TotalUnattence = Attendances.Where(x => !x.IsAttence).Sum(x => x.Total);
                        }
                    });
                } catch (Exception ex) {
                    Mvx.Trace(ex.Message);
                    ViewModelStatus = new ViewModelStatus("加载新数据失败", false, MessageType.Error, TipsType.DialogDisappearAuto);
                } finally {
                    NumberOfFirstLoadThread -= 1;
                }
                ViewModelStatus = new ViewModelStatus("加载成功", false,
                    MessageType.Success,
                    TipsType.DialogDisappearAuto);
                base.FirstLoad();
            });
        }

        #region Property

        private int _attence;

        public int MasterId { get; set; }

        public int ClassId { get; set; }

        string _className;

        public string ClassName {
            get{ return _className; }
            set { 
                _className = value;
                RaisePropertyChanged(() => ClassName);
            } 
        }

        public int Total { get; set; }

        private int _totalTotal;

        public int TotalTotal {
            get { return _totalTotal; }
            set {
                _totalTotal = value;
                RaisePropertyChanged(() => TotalTotal);
            }
        }

        private int _totalAttence;

        public int TotalAttence {
            get { return _totalAttence; }
            set {
                _totalAttence = value;
                RaisePropertyChanged(() => TotalAttence);
            }
        }

        private int _toalUnattence;

        /// <summary>
        /// 未考勤人数
        /// </summary>
        public int TotalUnattence {
            get { return _toalUnattence; }
            set {
                _toalUnattence = value;
                RaisePropertyChanged(() => TotalUnattence);
            }
        }

        private int _totalAbsence;

        /// <summary>
        /// 缺席总人数
        /// </summary>
        public int TotalAbsence {
            get { return _totalAbsence; }
            set {
                _totalAbsence = value;
                RaisePropertyChanged(() => TotalAbsence);
            }
        }

        public int Attence {
            get { return _attence; }
            set {
                _attence = value;
                RaisePropertyChanged(() => Attence);
                RaisePropertyChanged(() => UnAttence);
            }
        }

        public int UnAttence {
            get { return Total - Attence; }
        }

        public bool IsAttence { get; private set; }

        private ObservableCollection<AttendanceMasterModel> attendances = new ObservableCollection<AttendanceMasterModel>();

        public ObservableCollection<AttendanceMasterModel> Attendances {
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
                        new {masterId = MasterId, classId = ClassId, date = DateTime.Now.Date}));
                return _showDetailCommand;
            }
        }

        public MvxCommand ShowUnAttenceChildrenCommand {
            get {
                _showUnAttenceChildrenCommand = _showUnAttenceChildrenCommand ??
                new MvxCommand(
                    () =>
                     ShowViewModel<UnattenceChildrenViewModel>(
                        new {date = DateTime.Now.Date}));
                return _showUnAttenceChildrenCommand;
            }
        }

        #endregion
    }
}