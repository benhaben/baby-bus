using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using BabyBus.Models;
using BabyBus.Models.Enums;
using BabyBus.Net.Attendance;
using BabyBus.Services;
using BabyBus.Utilities;
using BabyBus.ViewModels.Setting;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using System.Collections.Generic;

namespace BabyBus.ViewModels.Attendance {
    public class UnattenceChildrenViewModel : BaseViewModel {
        private readonly IAttendanceService service;
        private DateTime _date;

        public UnattenceChildrenViewModel() {
            service = Mvx.Resolve<IAttendanceService>();
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

        public void Init(DateTime date) {
            _date = date;
        }

        public override void Start() {
            base.Start();

            FirstLoad();
        }

        public override void FirstLoad() {
            IsLoading = true;
            ViewModelStatus = new ViewModelStatus("正在加载...", true, MessageType.Information, TipsType.DialogProgress);
            if (NumberOfFirstLoadThread > 0) {
                return;
            }
            NumberOfFirstLoadThread += 1;
            Task.Run(async () => {
                User user = BabyBusContext.UserAllInfo;
                try {
                    ApiResult<ChildModel> result = await service.GetAttendanceChildList(BabyBusContext.ClassId, _date, AttenceType.UnAttence);
                    if (!result.Status) {
                        ViewModelStatus = new ViewModelStatus(result.Message);
                    } else {
                        Children = new ObservableCollection<ChildModel>(result.Items);
                    }
                } catch (Exception ex) {
                    Debug.WriteLine(ex.Message);
                    ViewModelStatus = new ViewModelStatus("加载新数据失败", false, MessageType.Error, TipsType.DialogDisappearAuto);
                } finally {
                    NumberOfFirstLoadThread -= 1;
                }

                ViewModelStatus = new ViewModelStatus("加载成功", false, MessageType.Success, TipsType.DialogDisappearAuto);
                base.FirstLoad();
            });
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
                        new {childId = 0, isCheckout = false, json = SelectedCheckoutJson}));
                return showDetailCommand;
            }
        }

        #endregion
    }
}