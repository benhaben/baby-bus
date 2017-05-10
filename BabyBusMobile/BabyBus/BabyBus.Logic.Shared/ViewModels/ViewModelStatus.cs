
namespace BabyBus.Logic.Shared
{
    public class ViewModelStatus
    {
        public ViewModelStatus(string information) : this(information, false) { }
        public ViewModelStatus(string information, bool isRunning) : this(information, isRunning, MessageType.Information) { }
        public ViewModelStatus(string information, bool isRunning, MessageType outputType)
            : this(information, isRunning, outputType, TipsType.DialogDisappearAuto) { }

        public ViewModelStatus(
            string information
            , bool isRunning = false
            , MessageType outputType = MessageType.Information
            , TipsType tipsType = TipsType.DialogDisappearAuto) {
            MessageType = outputType;
            IsRunning = isRunning;
            Information = information;
            TipsType = tipsType;
            }

        public TipsType TipsType {
            get;
            set;
        }

        public MessageType MessageType { get; set; }

        public bool IsRunning {
            get;
            set;
        }

        public string Information {
            get;
            set;
        }
    }
}