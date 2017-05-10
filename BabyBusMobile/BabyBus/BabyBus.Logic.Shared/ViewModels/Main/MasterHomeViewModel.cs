using Cirrious.MvvmCross.ViewModels;
using BabyBusSSApi.ServiceModel.Enumeration;


namespace BabyBus.Logic.Shared
{
    public class MasterHomeViewModel : BaseViewModel
    {
        public NoticeType NoticeType { get; set; }

        public MasterHomeViewModel()
        {
            User = BabyBusContext.UserAllInfo;
        }

        private UserModel _user;

        public UserModel User
        {
            get { return _user; }
            set
            {
                _user = value;                
            }
        }
        //masterhomeVIewmodel 类的构造函数

        private MvxCommand _showAttenceCommand;
        private MvxCommand _showSendNoticeCommand;

        public MvxCommand ShowSendNoticeCommand
        {
            get
            {
                _showSendNoticeCommand = _showSendNoticeCommand ?? new MvxCommand(() =>
                    ShowViewModel<SendNoticeViewModel>(new { type = (int)NoticeType }));
                return _showSendNoticeCommand;
            }
        }
        //消息发送命令处理

        public MvxCommand ShowAttenceCommand
        {
            get
            {
                _showAttenceCommand = _showAttenceCommand ??
                new MvxCommand(() => ShowViewModel<AttendanceMasterViewModel>());
                return _showAttenceCommand;
            }
        }
    }
}
