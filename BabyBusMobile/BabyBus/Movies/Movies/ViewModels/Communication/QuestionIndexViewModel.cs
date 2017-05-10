using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyBus.Message;
using BabyBus.Models;
using BabyBus.Models.Communication;
using BabyBus.Net.Communication;
using BabyBus.Utilities;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;

namespace BabyBus.ViewModels.Communication {
    public class QuestionIndexViewModel : BaseViewModel {
        private IQuestionService service;
        private long _maxId;
        private long _minId;
        private bool _isEnd = false;

        public event EventHandler<List<QuestionDetailViewModel>> DataRefreshed;
        public event EventHandler<List<QuestionDetailViewModel>> DataLoadedMore;

        public QuestionIndexViewModel() {
            service = Mvx.Resolve<IQuestionService>();

            #if DEBUG1
            var list = new List<QuestionModel>();
            QuestionModel question;
            question = new QuestionModel {
                Content = "小孩痰多可否怎么办？多行显示，多行显示，多行显示，多行显示，多行显示，多行显示，多行显示",
                CreateTime = DateTime.Now,
                ChildName = "王大锤",
            };
            list.Add(question);
            question = new QuestionModel {
                Content = "小孩白天老瞌睡，晚上精神好怎么办？",
                Answers = new List<AnswerModel>() {
                    new AnswerModel {
                        UserName = "沈老师",
                        Content = "多行显示，多行显示，多行显示，多行显示，多行显示，多行显示，多行显示"
                    }
                },
                CreateTime = new DateTime(2015, 1, 2, 18, 20, 12),
                ChildName = "王大锤",
            };
            list.Add(question);

            Questions = GetQuestionDetails(list);
            #endif

            var messenger = Mvx.Resolve<IMvxMessenger>();

            messenger.Subscribe<QuestionMessage>(m => {
                var item = Questions.FirstOrDefault(x => x.Question.QuestionId == m.Question.QuestionId);
                if (item != null) {
                    item.Question = m.Question;
                }
            });
        }

        private List<QuestionDetailViewModel> questions = new List<QuestionDetailViewModel>();

        public List<QuestionDetailViewModel> Questions {
            get { return questions; }
            set {
                questions = value;
                RaisePropertyChanged(() => Questions);
            }
        }

        private QuestionDetailViewModel selectedQuestion;

        public QuestionDetailViewModel SelectedQuestion {
            get { return selectedQuestion; }
            set {
                selectedQuestion = value;
            }
        }

        public override void Start() {
            base.Start();
            FirstLoad();
        }

        public void Init() {
            FirstLoad();
        }


        public override void FirstLoad() {
            IsLoading = true;

            Task.Run(async () => {
                var list = BabyBusContext.QuestionList.OrderByDescending(x => x.CreateTime).Take(PageSize).ToList();
                if (list.Count == 0) {
                    await LoadNewQuestions();
                } else {
                    _maxId = list.FirstOrDefault().QuestionId;
                    _minId = list.LastOrDefault().QuestionId;
                    Questions = GetQuestionDetails(list);
                }
                base.FirstLoad();
            });
        }

        private async Task<List<QuestionDetailViewModel>> LoadNewQuestions() {
            if (NumberOfLoadingNewNoticesThread > 0) {
                return new List<QuestionDetailViewModel>();
            }
            NumberOfLoadingNewNoticesThread += 1;
            var user = BabyBusContext.UserAllInfo;
            try {
                ViewModelStatus = new ViewModelStatus("正在加载新数据...", true, MessageType.Information, TipsType.DialogProgress);
                var result = await service.GetNewQuestionList(user, _minId);
                ViewModelStatus = new ViewModelStatus("加载新数据成功", false, MessageType.Success, TipsType.DialogDisappearAuto);

                if (!result.Status) {
                    ViewModelStatus = new ViewModelStatus(result.Message);
                } else {
                    if (result.Items.Count > 0) {
                        var updatedList = GetQuestionDetails(result.Items);
                        //make iOS bug
//                        Questions = updatedList;
                        BabyBusContext.InsertQuestions(result.Items);
                        Questions = GetQuestionDetails(BabyBusContext.QuestionList);

                        _maxId = result.Items.First().QuestionId;
                        if (Questions.Count == updatedList.Count)
                            _minId = result.Items.Last().QuestionId;
                    }
                }
            } catch (Exception ex) {
                ViewModelStatus = new ViewModelStatus("加载新数据失败", false, MessageType.Error, TipsType.DialogDisappearAuto);
                Debug.WriteLine(ex.Message);
            } finally {
                NumberOfLoadingNewNoticesThread -= 1;
            }
            return new List<QuestionDetailViewModel>();
        }

        private async Task<List<QuestionDetailViewModel>> LoadMoreQuestions() {
            if (_isEnd)
                return new List<QuestionDetailViewModel>();


            if (NumberOfLoadingMoreNoticesThread > 0) {
                return new List<QuestionDetailViewModel>();
            }
            NumberOfLoadingMoreNoticesThread += 1;

            ViewModelStatus = new ViewModelStatus("正在加载历史数据...", true, MessageType.Information, TipsType.DialogProgress);
            var user = BabyBusContext.UserAllInfo;
            try {
                var result = await service.GetOldQuestionList(user, _minId);
                ViewModelStatus = new ViewModelStatus("加载历史数据成功", false, MessageType.Success, TipsType.DialogDisappearAuto);

                if (!result.Status) {
                    ViewModelStatus = new ViewModelStatus(result.Message);
                } else {
                    if (result.Items.Count > 0) {
                        var addlist = GetQuestionDetails(result.Items);
                        foreach (var item in addlist) {
                            if (Questions.Count(x => x.Question.QuestionId == item.Question.QuestionId) == 0)
                                Questions.Add(item);
                        }
                        BabyBusContext.InsertQuestions(result.Items);
                        _minId = result.Items.Last().QuestionId;
                        return addlist;
                    } else {
                        _isEnd = true;
                    }
                }
            } catch (Exception ex) {
                Debug.WriteLine(ex.Message);
                ViewModelStatus = new ViewModelStatus("加载历史数据失败", false, MessageType.Error, TipsType.DialogDisappearAuto);
            } finally {
                NumberOfLoadingMoreNoticesThread -= 1;
            }
            return new List<QuestionDetailViewModel>();
        }

        private IMvxCommand refreshCommand;

        public IMvxCommand RefreshCommand {
            get {
                refreshCommand = refreshCommand ?? new MvxAsyncronizeCommand(async () => {
                    var addList = await LoadNewQuestions();
                    if (DataRefreshed != null) {
                        DataRefreshed(this, addList);
                    }
                });
                return refreshCommand;
            }
        }

        private IMvxCommand loadMoreCommand;

        public IMvxCommand LoadMoreCommand {
            get {
                loadMoreCommand = loadMoreCommand ?? new MvxAsyncronizeCommand(async () => {
                    var addList = await LoadMoreQuestions();
                    if (DataLoadedMore != null) {
                        DataLoadedMore(this, addList);
                    }
                });
                return loadMoreCommand;
            }
        }

        private List<QuestionDetailViewModel> GetQuestionDetails(List<QuestionModel> questions) {
            var list = new List<QuestionDetailViewModel>();
            foreach (var question in questions.OrderByDescending(x=>x.CreateTime)) {
                var vm = new QuestionDetailViewModel(question);
                list.Add(vm);
            }
            return list;
        }
    }
}
