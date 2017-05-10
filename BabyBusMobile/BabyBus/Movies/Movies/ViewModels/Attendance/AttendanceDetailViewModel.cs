using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BabyBus.Models;
using BabyBus.Models.Attendance;
using BabyBus.Net.Attendance;
using BabyBus.Services;
using BabyBus.Utilities;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;

namespace BabyBus.ViewModels.Attendance {
    public class AttendanceDetailViewModel : BaseViewModel {
        private DateTime _date;
        private int _masterId;
        private int _classId;
        private IAttendanceService _service;

        public AttendanceDetailViewModel() {
            _service = Mvx.Resolve<IAttendanceService>();
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

        public void Init(int masterId, int classId, DateTime date) {
            _masterId = masterId;
            _classId = classId;
            _date = date;
        }

        public override void Start() {
            base.Start();
            FirstLoad();
        }

        public override  void FirstLoad() {
            IsLoading = true;
            //ViewModelStatus = new ViewModelStatus("正在加载...", true, MessageType.Information, TipsType.DialogProgress);
            Task.Run(async () => {
                try {
                    var result = await _service.GetAttendanceChildList(_classId, _date);
                    if (!result.Status) {
                        ViewModelStatus = new ViewModelStatus(result.Message);
                    } else {
                        Children = new ObservableCollection<ChildModel>(result.Items);
                    }
                } catch (Exception ex) {
                    Debug.WriteLine(ex.Message);
                }
                base.FirstLoad();
                //ViewModelStatus = new ViewModelStatus("加载成功", false, MessageType.Success, TipsType.DialogDisappearAuto);
            });
           
        }

        #region Property

        private ObservableCollection<ChildModel> children = new ObservableCollection<ChildModel>();

        public ObservableCollection<ChildModel> Children {
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

        #endregion

        #region Command

        private MvxCommand _attenceCommand;

        public MvxCommand AttenceCommand {
            get {
                _attenceCommand = _attenceCommand ?? new MvxCommand(async() => {
                    var attChildren = (from child in Children
                                                      where child.IsSelect
                                                      select child.ChildId).ToList();

                    var model = new AttendanceMasterModel {
                        MasterId = _masterId,
                        ClassId = BabyBusContext.ClassId,
                        KindergartenId = BabyBusContext.KindergartenId,
                        Total = Children.Count,
                        Attence = attChildren.Count,
                        AttChildren = attChildren,
                        CreateDate = DateTime.Now.Date
                    };
                    try {
                        ViewModelStatus = new ViewModelStatus("考勤中...", true);
                        ApiResponser result = await _service.WorkAttendance(model);
                        if (result.Status) {
                            ViewModelStatus = new ViewModelStatus("考勤已经完成!", false, MessageType.Success, TipsType.DialogDisappearAuto);
                            Close(this);
                        } else {
                            ViewModelStatus = new ViewModelStatus(result.Message, false, MessageType.Error, TipsType.DialogWithCancelButton);
                        }

                    } catch (Exception ex) {
                        //send log to remote server
                    }

                });
                return _attenceCommand;
            }
        }

        #endregion
    }
}