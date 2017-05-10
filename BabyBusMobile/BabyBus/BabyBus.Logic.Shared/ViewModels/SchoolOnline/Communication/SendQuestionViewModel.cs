using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Plugins.Messenger;
using BabyBus.Logic.Shared;
using BabyBusSSApi.ServiceModel.Enumeration;
using Newtonsoft.Json;
using BabyBusSSApi.ServiceModel.DTO.Create;




namespace BabyBus.Logic.Shared
{

	public class SendQuestionViewModel : BaseViewModel
	{
		private IRemoteService _service;
		private IMvxMessenger _messenger;
		private MvxSubscriptionToken _token;
		RoleType _sendToWho = RoleType.Teacher;

		List<string> _sickNames = null;

		public List<string> SickNames {
			get {
				if (_sickNames == null) {
					_sickNames = new List<string>();
					_sickNames.Add("待确诊");
					_sickNames.Add("流感");
					_sickNames.Add("麻疹");
					_sickNames.Add("腮腺炎");
					_sickNames.Add("痢疾");
					_sickNames.Add("手足口");
				}
				return _sickNames;
			}
		}

		List<string> _sickMessage = null;

		public List<string> SickMessage {
			get {
				if (_sickMessage == null) {
					_sickMessage = new List<string> { 
						"我家宝宝生病了，需要请假！病因：待确诊",
						"我家宝宝生病了，需要请假！病因：流感",
						"我家宝宝生病了，需要请假！病因：麻疹",
						"我家宝宝生病了，需要请假！病因：腮腺炎",
						"我家宝宝生病了，需要请假！病因：痢疾",
						"我家宝宝生病了，需要请假！病因：手足口"
					};
				}
				return _sickMessage;
			}
		}



		public RoleType SendToWho {
			get {
				return _sendToWho;
			}
			private set {
				_sendToWho = value;
			}
		}

		public SendQuestionViewModel()
		{
			_service = Mvx.Resolve<IRemoteService>();
			_messenger = Mvx.Resolve<IMvxMessenger>();

			BeginDate = DateTime.Now.Date;
			EndDate = DateTime.Now.Date;

			SelectMemoType = new MemoType { Type = QuestionType.NormalMessage };

			_token = _messenger.Subscribe<ChildrenMessage>((message) => {
				Children = message.Children;
				var text = new StringBuilder();
				text.Append("发送给：");
				foreach (ChildModel item in Children) {
					text.Append(item.ChildName);
					text.Append("； ");
				}
				SelectedText = text.ToString();

				if (SelectChildrenEventHandler != null) {
					SelectChildrenEventHandler(null, null);
				}
			});
		}

		public event EventHandler SelectChildrenEventHandler;


		public void Init(RoleType isSendToWho)
		{
			SendToWho = isSendToWho;
		}

		#region Property


		private string content = string.Empty;

		public string Content {
			get { return content; }
			set {
				content = value;
				RaisePropertyChanged(() => Content);
			}
		}

		public List<ChildModel> Children{ get; set; }

		private string selectedText = string.Empty;

		public string SelectedText {
			get { return selectedText; }
			set {
				selectedText = value;
				RaisePropertyChanged(() => SelectedText);
			}
		}


		public string ContentHolder {
			get {
				if (SendToWho == RoleType.HeadMaster) {
					return "请输入给园长留言的信息...";
				} else if (SendToWho == RoleType.Teacher) {
					return
						SelectMemoType.Type == QuestionType.NormalMessage
                        ? "请输入给老师留言的信息..."
                        : "请输入请假原因以及请假时间……";
				} else if (SendToWho == RoleType.Parent)
					return "请输入给家长的留言信息……";
				else
					return "请输入留言信息……";
			}
		}

		private MemoType _selectMemoType;

		public MemoType SelectMemoType {
			get { return _selectMemoType; }
			set {
				_selectMemoType = value;
				RaisePropertyChanged(() => ContentHolder);
				RaisePropertyChanged(() => SelectMemoType);
			}
		}

		public DateTime BeginDate{ get; set; }

		public DateTime EndDate{ get; set; }

		#endregion

		#region Command

		public MvxCommand SendCommand {
			get {
				return new MvxCommand(async () => {
					QuestionType type;
					if (SendToWho == RoleType.HeadMaster) {
						type = QuestionType.MasterMessage;
					} else if (SendToWho == RoleType.Parent) {
						type = QuestionType.PersonalMessage;
					} else {
						type = SelectMemoType.Type;
					}
					//Check

					if (Content == null || string.IsNullOrEmpty(Content.Trim())) {
						ViewModelStatus = new ViewModelStatus("内容不能为空格", false, MessageType.Error, TipsType.DialogDisappearAuto);
						return;
					}
					if (type == QuestionType.PersonalMessage && (Children == null || Children.Count == 0)) {
						ViewModelStatus = new ViewModelStatus("请至少选择一个孩子", false, MessageType.Error, TipsType.DialogDisappearAuto);
						return;
					}
					if (type == QuestionType.AskforLeave && BeginDate.Date > EndDate.Date) {
						ViewModelStatus = new ViewModelStatus("开始时间不能大于结束时间", false, MessageType.Error, TipsType.DialogDisappearAuto);
						return;
					}
					if (type == QuestionType.AskforLeave && BeginDate.Date < DateTime.Now.Date) {
						ViewModelStatus = new ViewModelStatus("请假时间不能晚于今日", false, MessageType.Error, TipsType.DialogDisappearAuto);
						return;
					}

					ViewModelStatus = new ViewModelStatus(UIConstants.SENDING, true, MessageType.Information, TipsType.DialogProgress);

					var question = new CreateQuestion {
						ChildId = BabyBusContext.ChildId,
						Content = Content,
						QuestionType = type,
						BeginDate = BeginDate,
						EndDate = EndDate,
					};

					try {
						if (type == QuestionType.PersonalMessage) {
							question.Children = Children.Select(x => x.ChildId).ToList();
						}
						await _service.SendQuestion(question);
						ViewModelStatus = new ViewModelStatus(UIConstants.SEND_SUCCESS, true, MessageType.Success, TipsType.DialogDisappearAuto);
						Redirect();

					} catch (Exception ex) {
						ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Error);

					}
                       
				});
			}
		}

		void Redirect()
		{
			Task.Delay(1000).ContinueWith(task => {
				this.InvokeOnMainThread(() => Close(this));
			});
		}

		private MvxCommand _showSelectChildrenCommand;

		public MvxCommand ShowSelectChildrenCommand {
			get {
				_showSelectChildrenCommand = _showSelectChildrenCommand ??
				new MvxCommand(() => {
					var jsonString = JsonConvert.SerializeObject(this.Children);
					ShowViewModel<SelectChildrenViewModel>(new {jsonString = jsonString});
				});
				return _showSelectChildrenCommand;
			}
		}

		#endregion
	}

	public class MemoType
	{
		public QuestionType Type { get; set; }

		public string Text {
			get {
				switch (Type) {
					case QuestionType.AskforLeave:
						return "请假";
					case QuestionType.NormalMessage:
						return "留言";
					case QuestionType.MasterMessage:
						return "园长信箱";
					case QuestionType.PersonalMessage:
						return "个人留言";
					default:
						return "留言";                        
				}
			}
		}
	}
}
