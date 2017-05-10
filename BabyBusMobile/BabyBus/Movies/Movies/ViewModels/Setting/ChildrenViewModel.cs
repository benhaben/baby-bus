using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using BabyBus.Models;
using BabyBus.Net.Main;
using BabyBus.Services;
using BabyBus.Utilities;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;

namespace BabyBus.ViewModels.Setting {
    public class ChildrenViewModel : BaseViewModel {
        private readonly IChildService service;

        public ChildrenViewModel() {
            service = Mvx.Resolve<IChildService>();
        }

        /// <summary>
        ///     Selected Checkout Object, Convert To Json for Detail Page
        /// </summary>
        public string SelectedCheckoutJson { get; set; }

        #region Property

        private ObservableCollection<ChildModel> children = new ObservableCollection<ChildModel>();

        public ObservableCollection<ChildModel> Children {
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
                    ApiResult<ChildModel> result = await service.GetByClassId(user.ClassId);
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
    }
}