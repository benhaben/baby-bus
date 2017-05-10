using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyBus.Models.Communication;
using BabyBus.Net.Communication;
using BabyBus.Utilities;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using Newtonsoft.Json;

namespace BabyBus.ViewModels.Communication {

    public class SendQuestionViewModel : BaseViewModel {
        private IQuestionService _service;

        public SendQuestionViewModel() {
            _service = Mvx.Resolve<IQuestionService>();

            QuestionTypes = new List<MemoType> {
                new MemoType {Type = QuestionType.NormalMessage},
                new MemoType {Type = QuestionType.AskforLeave}
            };

            SelectMemoType = new MemoType {Type = QuestionType.NormalMessage};
        }

        public void Init() {
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

        private string contentHolder = "请输入问题的内容，最多一千个字...";

        public string ContentHolder {
            get {
                return
                    SelectMemoType.Type == QuestionType.NormalMessage
                        ? "请输入给老师留言的信息..."
                        : "请告诉我们孩子请假时间以及原因，例如：事假，病假（如为病假，请爸爸妈妈们确认是否有以下症状：1、手足口；2、流感；3、麻疹；4、腮腺炎；5、待确诊，及时给宝宝给予治疗，如有问题请与老师取得联系）";
            }
        }

        public List<MemoType> QuestionTypes { get; set; }

        private MemoType _selectMemoType;

        public MemoType SelectMemoType {
            get { return _selectMemoType; }
            set {
                _selectMemoType = value;
                RaisePropertyChanged(() => ContentHolder);
                RaisePropertyChanged(() => SelectMemoType);
            }
        }
        #endregion

        #region Command

        public MvxCommand SendCommand {
            get {
                return new MvxCommand(async () => {
                    //Check

                    if (string.IsNullOrEmpty(Content)) {
                        ViewModelStatus = new ViewModelStatus("请输入问题的内容");
                        return;
                    }
                    if (string.IsNullOrEmpty(Content.Trim())) {
                        ViewModelStatus = new ViewModelStatus("内容不能为空格");
                        return;
                    }

                    ViewModelStatus = new ViewModelStatus("正在发送...", true, MessageType.Information, TipsType.DialogProgress);

                    var question = new QuestionModel {
                        ChildId = BabyBusContext.ChildId,
                        Content = Content,
                        QuestionType = SelectMemoType.Type
                    };
                    try {
                        var result = await _service.SendQuestion(question);
                        if (result.Status) {
                            //question = JsonConvert.DeserializeObject<QuestionModel>(result.Attach.ToString());
                            ViewModelStatus = new ViewModelStatus("发送成功");
                            Close(this);
                        } else {
                            ViewModelStatus = new ViewModelStatus(result.Message, false, MessageType.Error,
                                TipsType.DialogWithOkButton);
                        }
                    } catch (Exception ex) {
                        Debug.WriteLine(ex.Message);
                    }
                });
            }
        }

        #endregion
    }

    public class MemoType {        
        public QuestionType Type { get; set; }

        public string Text {
            get {
                switch (Type) {
                        case QuestionType.AskforLeave:
                        return "请假";
                        case QuestionType.NormalMessage:
                        return "留言";
                    default:
                        return "留言";                        
                }
            }
        }
    }
}
