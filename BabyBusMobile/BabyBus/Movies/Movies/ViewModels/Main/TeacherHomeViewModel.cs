using BabyBus.Models.Enums;
using BabyBus.ViewModels.Attendance;
using BabyBus.ViewModels.Communication;
using Cirrious.MvvmCross.ViewModels;

namespace BabyBus.ViewModels.Main {
    public class TeacherHomeViewModel : BaseViewModel {
        public NoticeType NoticeType { get; set; }

        #region Command

        private MvxCommand _showAttenceCommand;
        private MvxCommand _showSendNoticeCommand;

        public MvxCommand ShowSendNoticeCommand {
            get {
                _showSendNoticeCommand = _showSendNoticeCommand ?? new MvxCommand(() =>
                    ShowViewModel<SendNoticeViewModel>(new {type = (int)NoticeType}));
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

        #endregion
    }
}