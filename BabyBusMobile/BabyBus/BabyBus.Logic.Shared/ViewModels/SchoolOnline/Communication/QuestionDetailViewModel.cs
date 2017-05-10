using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;



using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using BabyBusSSApi.ServiceModel.Enumeration;
using BabyBusSSApi.ServiceModel.DTO.Create;



namespace BabyBus.Logic.Shared
{
	public class QuestionDetailViewModel : BaseViewModel
	{
		private int _questionId;
		private bool _isLoadRemote;
		private IRemoteService _service;
		private readonly IMvxMessenger messenger;

		public event EventHandler RefreshAnswers;

		public QuestionDetailViewModel()
		{
			messenger = Mvx.Resolve<IMvxMessenger>();
		}

		public QuestionDetailViewModel(QuestionModel question)
			: this()
		{
			Question = question;
		}

		public void Init(int questionId, bool isLoadRemote = false)
		{
			_questionId = questionId;
			_isLoadRemote = isLoadRemote;
		}

		public override void InitData()
		{
			base.InitData();
			_service = Mvx.Resolve<IRemoteService>();
			var questions = BabyBusContext.QuestionList;
			Question = questions.FirstOrDefault(x => x.QuestionId == _questionId);
			//拿到Answers 
			try {
				ViewModelStatus = new ViewModelStatus(UIConstants.LOADING, true, MessageType.Information, TipsType.DialogProgress);
				if (Question == null) {
					var result = _service.GetQuestionById(this._questionId).Result;
					Question = result;
				}

				Question.Answers = _service.GetAnswersByQuestionId(_questionId).Result;
				ViewModelStatus = new ViewModelStatus(UIConstants.LOAD_SUCCESS, false, MessageType.Success, TipsType.Undisplay);
				BabyBusContext.InsertQuestions(new List<QuestionModel>(){ Question });

			} catch (Exception ex) {
				ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Success, TipsType.Undisplay);
			}
		}

		/// <summary>
		/// The question. when you want to show the answers, get them from QuestionModel
		/// </summary>
		private QuestionModel question;

		public QuestionModel Question {
			get { return question; }
			set {
				question = value;
				RaisePropertyChanged(() => Question);
			}
		}

		public QuestionType QuestionType {
			get { return question.QuestionType; }
		}

		/// <summary>
		/// The answer. use when teacher want to send an answer
		/// </summary>
		private string answer;

		public string Answer {
			get { return answer; }
			set {
				answer = value;
				RaisePropertyChanged(() => Answer);
			}
		}

		private IMvxCommand sendAnswerCommand;

		public IMvxCommand SendAnswerCommand {
			get {
				sendAnswerCommand = sendAnswerCommand ?? new MvxCommand(async () => {
                    
					//Check
					if (string.IsNullOrEmpty(Answer)) {
						return;
					}

					var answer = new CreateAnswer {
						Content = Answer,
						QuestionId = _questionId,
					};
					try {
						ViewModelStatus = new ViewModelStatus(UIConstants.SENDING, true, MessageType.Information, TipsType.DialogProgress);
						await _service.SendAnswer(answer);

						ViewModelStatus = new ViewModelStatus(UIConstants.SEND_SUCCESS, false, MessageType.Information, TipsType.Undisplay);

						//Local
						if (Question.Answers == null) {
							Question.Answers = new List<AnswerModel>();
						}                                   
						Question.Answers.Add(new AnswerModel {
							UserId = BabyBusContext.UserId,
							Content = Answer,
							QuestionId = _questionId,
							RoleType = BabyBusContext.RoleType,
							RealName = BabyBusContext.UserAllInfo.RealName,
							ImageName = BabyBusContext.UserAllInfo.ImageName,
						});
						BabyBusContext.Insert(answer);
						Answer = string.Empty;
						messenger.Publish(new QuestionMessage(this, Question));
						//Refresh Content
						if (RefreshAnswers != null) {
							RefreshAnswers.Invoke(null, null);
						}
                         
					} catch (Exception ex) {
						ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Information, TipsType.Undisplay);
					}
				});
				return sendAnswerCommand;
			}
		}
	}
}
