using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyBus.Helpers;
using BabyBus.Models;
using BabyBus.Models.Enums;
using BabyBus.Utilities;
using BabyBus.ViewModels.Attendance;
using BabyBus.ViewModels.Communication;
using Cirrious.MvvmCross.ViewModels;

namespace BabyBus.ViewModels.Main {
    public class MasterHomeViewModel : BaseViewModel {
        public NoticeType NoticeType { get; set; }

        public MasterHomeViewModel() {
            User = BabyBusContext.UserAllInfo;
        }

        private User _user;

        public User User {
            get { return _user; }
            set {
                _user = value;                
            }
        }

        private MvxCommand _showAttenceCommand;
        private MvxCommand _showSendNoticeCommand;

        public MvxCommand ShowSendNoticeCommand {
            get {
                _showSendNoticeCommand = _showSendNoticeCommand ?? new MvxCommand(() =>
                    ShowViewModel<SendNoticeViewModel>(new { type = (int)NoticeType }));
                return _showSendNoticeCommand;
            }
        }

        public MvxCommand ShowAttenceCommand {
            get {
                _showAttenceCommand = _showAttenceCommand ??
                new MvxCommand(() => ShowViewModel<AttendanceMasterViewModel>());
                return _showAttenceCommand;
            }
        }
    }
}
