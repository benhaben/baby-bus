using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;




using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using System.Threading;


namespace BabyBus.Logic.Shared
{
	public class QuestionIndexViewModel : BaseListViewModel
	{
		private IRemoteService _service;
		private bool _isEnd = false;

		private long _maxId = 0;
		//在多线程环境中
		public long MaxId {
			get {
				return Interlocked.Read(ref _maxId);
			}
			set {
				Interlocked.Exchange(ref _maxId, value);
			}
		}

		//当前最小的id
		private long _minId = 0;

		public long MinId {
			get {
				return Interlocked.Read(ref _minId);
			}
			set {
				//better to use Interlocked.Increment(), but this way is OK
				Interlocked.Exchange(ref _minId, value);
			}
		}

		public event EventHandler<List<QuestionModel>> DataRefreshed;
		public event EventHandler<List<QuestionModel>> DataLoadedMore;

		public QuestionIndexViewModel()
		{
			_service = Mvx.Resolve<IRemoteService>();

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
		}

		//        private List<QuestionDetailViewModel> questions = new List<QuestionDetailViewModel>();
		//
		//        public List<QuestionDetailViewModel> Questions {
		//            get { return questions; }
		//            set {
		//                questions = value;
		//                RaisePropertyChanged(() => Questions);
		//            }
		//        }

		private QuestionDetailViewModel _selectedQuestion;

		public QuestionDetailViewModel SelectedQuestion {
			get { return _selectedQuestion; }
			set {
				_selectedQuestion = value;
			}
		}

		//线程同步：一是互斥/加锁，目的是保证临界区代码操作的“原子性”；
		//另一种是信号灯操作，目的是保证多个线程按照一定顺序执行，如生产者线程要先于消费者线程执行
		//InitData LoadNewNotices LoadMoreNotices都需要使用_maxId，_minId,这里使用lock确保他们不同时操作
		//lock 关键字将语句块标记为临界区
		private Object _idLock = new Object();

		//确保InitData先于 RefreshCommand 调用
		private AutoResetEvent _autoevent = new AutoResetEvent(false);

		public override void InitData() {
			base.InitData();
          
			try {
				var list = BabyBusContext.QuestionList.OrderByDescending(x => x.CreateTime).Take(PageSize).ToList();
				if (list.Count == 0) {
					var task = Task.Factory.StartNew(() => {
						ListObject = LoadNewQuestionsCore(true).Result;
					});
					task.Wait();
                        
				} else {
					lock (_idLock) {
						MaxId = list.FirstOrDefault().QuestionId;
						MinId = list.LastOrDefault().QuestionId;
						ListObject = list;
					}
                        
					_autoevent.Set();
				}
			} catch (Exception ex) {
				Mvx.Trace(ex.Message);
			}
		}

		async Task<List<QuestionModel>> LoadNewQuestionsCore(bool isFirst) {
			var user = BabyBusContext.UserAllInfo;
			try {
				ViewModelStatus = new ViewModelStatus(UIConstants.LOADING, true, MessageType.Information, isFirst ? TipsType.DialogProgress : TipsType.Undisplay);
				var questions = await _service.GetNewQuestionList(_minId, user.RoleType);
				ViewModelStatus = new ViewModelStatus(UIConstants.LOAD_SUCCESS, false, MessageType.Success, TipsType.Undisplay);
               
				if (questions.Count > 0) {
					lock (_idLock) {
						BabyBusContext.InsertQuestions(questions);
						MaxId = questions.First().QuestionId;
						if (MinId == 0)
							MinId = questions.Last().QuestionId;
						return questions;
					}
				}
			} catch (Exception ex) {
				ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Error, TipsType.DialogDisappearAuto);
				Debug.WriteLine(ex.Message);
			} finally {
				_autoevent.Set();
			}
			return new List<QuestionModel>();
		}

		private async Task<List<QuestionModel>> LoadNewQuestions() {
			_autoevent.WaitOne();
			return await LoadNewQuestionsCore(false);
		}

		private async Task<List<QuestionModel>> LoadMoreQuestions() {
			if (_isEnd)
				return new List<QuestionModel>();
           
			ViewModelStatus = new ViewModelStatus(UIConstants.LOADING, true, MessageType.Information, TipsType.Undisplay);
			var user = BabyBusContext.UserAllInfo;
			try {
//                ViewModelStatus = new ViewModelStatus(UIConstants.LOADING, true, MessageType.Information, TipsType.DialogProgress);
				var questions = await _service.GetOldQuestionList(_minId, user.RoleType);
				ViewModelStatus = new ViewModelStatus(UIConstants.LOAD_SUCCESS, false, MessageType.Success, TipsType.Undisplay);

               
				if (questions.Count > 0) {
					lock (_idLock) {
						BabyBusContext.InsertQuestions(questions);
						MinId = questions.Last().QuestionId;
						return questions;
					}
				} else {
					_isEnd = true;
				}
			} catch (Exception ex) {
				ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Error, TipsType.DialogDisappearAuto);
			} finally {
			}
			return new List<QuestionModel>();
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

		public void ShowDetailCommand(long questionId) {
			ShowViewModel<QuestionDetailViewModel>(new { questionId = questionId});
		}
	}
}
