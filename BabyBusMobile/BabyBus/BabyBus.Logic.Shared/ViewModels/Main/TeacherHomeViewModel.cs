using BabyBus.Logic.Shared;
using BabyBusSSApi.ServiceModel.Enumeration;
using Cirrious.MvvmCross.ViewModels;

namespace BabyBus.Logic.Shared
{
    public class TeacherHomeViewModel : BaseViewModel
    {
        public NoticeType NoticeType { get; set; }

        #region Command

        private MvxCommand _showAttenceCommand;
        private MvxCommand _showSendNoticeCommand;
        private MvxCommand _showSendQuestionCommand;

        public MvxCommand ShowSendNoticeCommand
        {
            get
            {
                _showSendNoticeCommand = _showSendNoticeCommand ?? new MvxCommand(() =>
                    ShowViewModel<SendNoticeViewModel>(new {type = (int)NoticeType}));
                return _showSendNoticeCommand;
            }
        }
        //俘获命令事件，并返回命令格式

        public MvxCommand ShowAttenceCommand
        {
            get
            {
                _showAttenceCommand = _showAttenceCommand ??
                new MvxCommand(() => ShowViewModel<AttendanceMasterViewModel>());
                return _showAttenceCommand;
            }
        }

        public MvxCommand ShowSendQuestionCommand
        {
            get
            {
                _showSendQuestionCommand = _showSendQuestionCommand ??
                new MvxCommand(() => ShowViewModel<SendQuestionViewModel>(new {isSendToWho = (int)RoleType.Parent}));
                return _showSendQuestionCommand;
            }
        }

        MvxCommand _showMultipleIntelligenceCommand;

        public MvxCommand ShowMultipleIntelligenceCommand
        {
            get
            { 
                _showMultipleIntelligenceCommand = _showMultipleIntelligenceCommand ??
                new MvxCommand(() => ShowViewModel<TeacherModalityViewModel>());
                return _showMultipleIntelligenceCommand;
            }
        }

        #endregion
    }
}