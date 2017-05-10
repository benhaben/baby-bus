using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using BabyBus.Message;
using BabyBus.Models.Account;
using BabyBus.Net.Setting;
using BabyBus.Utilities;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;

namespace BabyBus.ViewModels.Setting {
    public class CheckoutIndexViewModel : BaseViewModel {
        private readonly ICheckoutService service;

        public CheckoutIndexViewModel() {
            service = Mvx.Resolve<ICheckoutService>();

            var messenger = Mvx.Resolve<IMvxMessenger>();

            messenger.Subscribe<CheckoutMessage>(m => {
                var checkout = Checkouts.FirstOrDefault(x => x.CheckoutId == m.Checkout.CheckoutId);
                if (checkout != null) {
                    checkout.AuditType = m.Checkout.AuditType;
                }
            });
        }

        /// <summary>
        /// Selected Checkout Object, Convert To Json for Detail Page
        /// </summary>
        public string SelectedCheckoutJson { get; set; }

        public override void Start() {
            base.Start();

            FirstLoad();
        }

        public  override void FirstLoad() {
            IsLoading = true;
            ViewModelStatus = new ViewModelStatus("正在加载...", true, MessageType.Information, TipsType.DialogProgress);

            Task.Run(async () => {
                var user = BabyBusContext.UserAllInfo;
                try {
                    var result = await service.GetCheckoutList(user);
                    if (!result.Status) {
                        ViewModelStatus = new ViewModelStatus(result.Message);
                    } else {
                        Checkouts = new ObservableCollection<CheckoutModel>(result.Items);
                    }
                } catch (Exception ex) {
                    Debug.WriteLine(ex.Message);
                }
                base.FirstLoad();
                ViewModelStatus = new ViewModelStatus("加载成功", false, MessageType.Success, TipsType.DialogDisappearAuto);
            });

        }

        #region Property

        private ObservableCollection<CheckoutModel> checkouts = new ObservableCollection<CheckoutModel>();

        public ObservableCollection<CheckoutModel> Checkouts {
            get { return checkouts; }
            set {
                checkouts = value;
                RaisePropertyChanged(() => Checkouts);
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
                        new {isCheckout = true, json = SelectedCheckoutJson}));
                return showDetailCommand;
            }
        }

        #endregion
    }
}