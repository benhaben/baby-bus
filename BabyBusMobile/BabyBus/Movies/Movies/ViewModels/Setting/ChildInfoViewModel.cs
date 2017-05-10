using System;
using System.IO;
using BabyBus.Message;
using BabyBus.Models;
using BabyBus.Models.Account;
using BabyBus.Models.Enums;
using BabyBus.Net.Setting;
using BabyBus.Services;
using BabyBus.Utilities;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using Newtonsoft.Json;

namespace BabyBus.ViewModels.Setting {
    public class ChildInfoViewModel : BaseViewModel {
        private readonly IMvxMessenger messenger;
        private readonly ICheckoutService service;

        private readonly IPictureService pic;

        public ChildInfoViewModel() {
            service = Mvx.Resolve<ICheckoutService>();
            messenger = Mvx.Resolve<IMvxMessenger>();
            pic = Mvx.Resolve<IPictureService>();
        }

        public event EventHandler SuccessedAudit;

        public void Init(bool isCheckout = false, string json = "") {
            _isCheckout = isCheckout;
            if (isCheckout) {
                checkout = JsonConvert.DeserializeObject<CheckoutModel>(json);
                ChildName = checkout.ChildName;
                GenderName = checkout.GenderName;
                Birthday = checkout.Birthday;
                ParentName = checkout.RealName;
                PhoneNumber = checkout.LoginName;
                Address = "";
                CheckoutAuditType = checkout.AuditType;
                ImageName = checkout.ImageName;
            } else {
                child = JsonConvert.DeserializeObject<ChildModel>(json);
                ChildName = child.ChildName;
                GenderName = child.GenderName;
                Birthday = child.Birthday;
                ParentName = child.ParentName;
                PhoneNumber = child.ParentPhone;
                Address = "";
                ImageName = child.ImageName;
            }
            ClassName = BabyBusContext.Class.ClassName;

            //Load Image
            //TODO: iOS don't need this
            pic.LoadIamgeFromSource(ImageName,
                stream => {
                    var ms = stream as MemoryStream;
                    if (ms != null)
                        Bytes = ms.ToArray();
                });
        }

        #region Property

        private bool _isCheckout;
        private CheckoutModel checkout;
        private AuditType checkoutAuditType = AuditType.Passed;
        private ChildModel child;
        private byte[] _bytes;

        public byte[] Bytes {
            get { return _bytes; }
            set {
                _bytes = value;
                RaisePropertyChanged(() => Bytes);
            }
        }

        public AuditType CheckoutAuditType {
            get { return checkoutAuditType; }
            private set { checkoutAuditType = value; }
        }

        public string ChildName { get; private set; }

        public string ClassName { get; private set; }

        public string GenderName { get; private set; }

        public DateTime Birthday { get; private set; }

        public string ParentName { get; private set; }

        public string PhoneNumber { get; private set; }

        public string Address { get; private set; }

        public string Memo { get; set; }

        public string ImageName { get; set; }

        #endregion

        #region Command

        public IMvxCommand ApproveCommand {
            get {
                return new MvxCommand(async () => {
                    ViewModelStatus = new ViewModelStatus("正在审批...", true);

                    checkout.AuditType = AuditType.Passed;
                    ApiResponser response = await service.Checkout(checkout);
                    if (response.Status) {
                        ViewModelStatus = new ViewModelStatus("操作成功");
                        if (SuccessedAudit != null) {
                            SuccessedAudit(null, null);
                        }
                    } else {
                        ViewModelStatus = new ViewModelStatus(response.Message);
                    }
                });
            }
        }

        public IMvxCommand RefuseCommand {
            get {
                return new MvxCommand(async () => {
                    ViewModelStatus = new ViewModelStatus("正在审批...", true);

                    checkout.AuditType = AuditType.Refused;
                    checkout.Memo = Memo;
                    ApiResponser response = await service.Checkout(checkout);
                    if (response.Status) {
                        ViewModelStatus = new ViewModelStatus("操作成功");
                        if (SuccessedAudit != null) {
                            SuccessedAudit(null, null);
                        }
                    } else {
                        ViewModelStatus = new ViewModelStatus(response.Message);
                    }
                });
            }
        }

        public IMvxCommand ReturnCommand {
            get {
                return new MvxCommand(() => {
                    if (_isCheckout)
                        messenger.Publish(new CheckoutMessage(this, checkout));
                });
            }
        }

        #endregion
    }
}