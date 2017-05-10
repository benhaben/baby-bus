using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BabyBus.Message;
using BabyBus.Models.Communication;
using BabyBus.Net.Communication;
using BabyBus.Utilities;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using Newtonsoft.Json;

namespace BabyBus.ViewModels.Communication {
    public class QuestionDetailViewModel : BaseViewModel {
        private int _questionId;
        private IQuestionService _service;
        private readonly IMvxMessenger messenger;

        public event EventHandler RefreshAnswers;

        public QuestionDetailViewModel() {
            messenger = Mvx.Resolve<IMvxMessenger>();
        }

        public QuestionDetailViewModel(QuestionModel question):this() {
            Question = question;
        }

        public void Init(int questionId) {
            _questionId = questionId;
        }

        public override void Start() {
            base.Start();

            _service = Mvx.Resolve<IQuestionService>();
            var questions = BabyBusContext.QuestionList;
            Question = questions.FirstOrDefault(x => x.QuestionId == _questionId);
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

        private IMvxCommand showDetailCommand;

        public IMvxCommand ShowDetailCommand {
            get {
                showDetailCommand = showDetailCommand ?? new MvxCommand(() => {
                    ShowViewModel<QuestionDetailViewModel>(new { questionId = Question.QuestionId });
                });
                return showDetailCommand;
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

                    var answer = new AnswerModel {
                        Content = Answer,
                        QuestionId = _questionId,
                        UserId = BabyBusContext.UserId,
                        RoleType = BabyBusContext.RoleType
                    };
                    try {
                        ViewModelStatus = new ViewModelStatus("正在发送...", true, MessageType.Information, TipsType.DialogProgress);
                        var result = await _service.SendAnswer(answer);
                        if (result.Status) {
                            answer = JsonConvert.DeserializeObject<AnswerModel>(result.Attach.ToString());
                            ViewModelStatus = new ViewModelStatus("发送成功!", false, MessageType.Information, TipsType.DialogDisappearAuto);

                            Answer = string.Empty;
                            //Local
                            answer.UserName = BabyBusContext.UserAllInfo.RealName;
                            if (Question.Answers == null)
                                Question.Answers = new List<AnswerModel>();
                            Question.Answers.Insert(0, answer);
                            BabyBusContext.Insert(answer);
                            messenger.Publish(new QuestionMessage(this,Question));
                            //Refresh Content
                            if (RefreshAnswers != null) {
                                RefreshAnswers.Invoke(null, null);
                            }
                        } else {
                            ViewModelStatus = new ViewModelStatus(result.Message, false, MessageType.Error,
                                TipsType.DialogWithOkButton);
                        }
                    } catch (Exception ex) {
                        Debug.WriteLine(ex.Message);
                    }
                });
                return sendAnswerCommand;
            }
        }
    }
}
