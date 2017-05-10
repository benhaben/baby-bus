using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BabyBus.Logic.Shared;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using BabyBusSSApi.ServiceModel.Enumeration;

namespace BabyBus.Logic.Shared
{
	public class SendNoticeViewModel : BaseViewModel
	{
		private readonly IRemoteService _remoteService;

		NoticeType _type;
		NoticeViewType _viewType;
		string selectedText = "全班";

		public SendNoticeViewModel(IMvxMessenger messenger)
		{
			_remoteService = Mvx.Resolve<IRemoteService>();

			messenger.Subscribe<ChildrenMessage>(m => {
				var text = new StringBuilder();
				List<ChildModel> args = m.Children;
				int selected = args.Count(x => x.IsSelect);
				IsAllClass = (args.Count == selected);
				if (!IsAllClass) {
					int i = 0;
					foreach (ChildModel item in args.Where(x => x.IsSelect)) {
						text.Append(item.ChildName);
						text.Append(" ");
						if (++i == 3) {
							break;
						}
					}

					if (selected > 3) {
						text.Append("0...");
					}
					SelectedText = text.ToString();
				} else {
					SelectedText = "全班";
				}
			});
		}

		public string SelectedText {
			get { return selectedText; }
			set {
				selectedText = value;
				RaisePropertyChanged(() => SelectedText);
			}
		}

		public bool IsAllClass { get; set; }

		/// <summary>
		/// Occurs when send images fininshed. subscribe by ui
		/// </summary>
		public event SendImagesResultCallBack SendImagesResultEventHandler;

		ImageSendHelper _imageHelp = new ImageSendHelper();

		public ImageSendHelper ImageHelper {
			get{ return _imageHelp; }
		}

		/// <summary>
		/// a call back function, get results of send images
		/// </summary>
        public delegate void SendImagesResultCallBack(IList<UploadImageData> successlist,IList<UploadImageData> failureList);


		public void Init(int type)
		{
			_type = (NoticeType)type;
		}

		public override void InitData()
		{
			base.InitData();
			//Init Title&Content
			string datestr = DateTime.Now.ToString("D");
			_viewType = NoticeViewType.Notice;
			if (_type == NoticeType.ClassHomework) {
				Title = string.Format("{0}的家庭作业", datestr);
			} else if (_type == NoticeType.ClassCommon) {
				Title = string.Format("{0}的班级通知", datestr);
			} else if (_type == NoticeType.KindergartenAll) {
				Title = string.Format("{0}的园区通知", datestr);
			} else if (_type == NoticeType.KindergartenStaff) {
				Title = string.Format("{0}的园务通知", datestr);
			} else if (_type == NoticeType.GrowMemory) {
				Title = string.Empty;
				_viewType = NoticeViewType.GrowMemory;
			} else if (_type == NoticeType.KindergartenRecipe) {
				Title = string.Format("{0}的食谱", datestr);
			} else {
				Title = string.Empty;
			}
		}

		#region Property

		private string content = string.Empty;
		private string contentHolder = "请输入内容，最多一千个字...";
		private string title;

		public string Title {
			get { return title; }
			set {
				title = value;
				RaisePropertyChanged(() => Title);
			}
		}

		public NoticeType NoticeType {
			get { return _type; }
		}

		public string Content {
			get { return content; }
			set {
				content = value;
				RaisePropertyChanged(() => Content);
			}
		}

		public string ContentHolder {
			get {
				if (_type == NoticeType.ClassCommon
				    || _type == NoticeType.ClassEmergency
				    || _type == NoticeType.ClassHomework
				    || _type == NoticeType.KindergartenAll
				    || _type == NoticeType.KindergartenStaff
				    || _type == NoticeType.GrowMemory) {
					this.contentHolder = "请输入内容，最多一千个字...";
				} else if (_type == NoticeType.KindergartenRecipe) {
					this.contentHolder = "请输入食谱...";
				}
				return this.contentHolder;
			}
		}

		public List<string> ImagesUrl {
			get;
			set;
		}

		#endregion

		private void GeneratorImagesNameIntoSendData(IList<UploadImageData> list)
		{
			if (_tempNoticeSave != null && list != null && list.Count > 0) {

				//如果以前有部分名字，加上一个，在末尾
				if (!string.IsNullOrWhiteSpace(_tempNoticeSave.NormalPics)) {
					_tempNoticeSave.NormalPics += ",";
				}

				//拼接字符串，用，分割
				foreach (var data in list) {
					_tempNoticeSave.NormalPics += data.RemoteImageName;
					_tempNoticeSave.NormalPics += ",";
				}

				//去掉一个，在末尾
				_tempNoticeSave.NormalPics = _tempNoticeSave.NormalPics.Remove(_tempNoticeSave.NormalPics.Length - 1);
			}
		}

		public void Close()
		{
			Close(this);
		}

		#region Command

		public Task SendNoticeAndImage(IList<UploadImageData> list)
		{

			return Task.Run(async () => {
				await UploadImages(list, delegate(IList<UploadImageData> successList, IList<UploadImageData> failureList) {
					if (failureList.Count == 0 && successList.Count > 0) {
						try {
							//Note: should use += here
							GeneratorImagesNameIntoSendData(successList);
							_remoteService.SendNotice(_tempNoticeSave);
							ViewModelStatus = new ViewModelStatus(UIConstants.SEND_SUCCESS, false, MessageType.Success);
							Redirect();
                                   
						} catch (Exception ex) {
							ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Error, TipsType.DialogWithOkButton);
						}
					} else {
						//a part of image upload success, so save them
						GeneratorImagesNameIntoSendData(successList);
					}
					//call back no matter success or failure
					if (SendImagesResultEventHandler != null) {
						SendImagesResultEventHandler(successList, failureList);
					}
				});
			});
		}

		/// <summary>
		/// when send notice, we keep the noticemodel, if send image fail, we can reuse the notice and send again
		/// </summary>
		private NoticeModel _tempNoticeSave = null;

		public void ClearDataAfterGiveUpSendNotice()
		{
			_tempNoticeSave = null;
			_tokenSource.Cancel();
			this.ImagesUrl.Clear();
			this.Close(this);
		}

		private MvxCommand _showChildrenPageCommand;
		private Task _uploadTask = null;

		void Redirect()
		{
			Task.Delay(1000).ContinueWith(task => {
				this.InvokeOnMainThread(() => {
					Close(this);
					var messenger = Mvx.Resolve<IMvxMessenger>();
					var msg = new RedirectMessage(null);

					if (this.NoticeType == NoticeType.ClassCommon
					    || this.NoticeType == NoticeType.ClassHomework
					    || this.NoticeType == NoticeType.KindergartenAll
					    || this.NoticeType == NoticeType.KindergartenStaff
					    || this.NoticeType == NoticeType.KindergartenRecipe) {
						msg.PageTag = "Notice";
					} else if (this.NoticeType == NoticeType.GrowMemory) {
						msg.PageTag = "GrowMemory";
					}
                           
					messenger.Publish<RedirectMessage>(msg);
				});
			});
		}

		public MvxCommand SendCommand {
			get {
				return new MvxCommand(async () => {
                  
					try {
						//0. Check

						if (Title == null || string.IsNullOrEmpty(Title.Trim())) {
							ViewModelStatus = new ViewModelStatus("请输入标题", false, MessageType.Error, TipsType.DialogWithOkButton);
							return;
						}
						if (_viewType != NoticeViewType.GrowMemory
						    && (Content == null || string.IsNullOrEmpty(Content.Trim()))) {
							ViewModelStatus = new ViewModelStatus("请输入内容", false, MessageType.Error, TipsType.DialogWithOkButton);
							return;
						}

						if (this.Title.Length > Constants.MaxTitleLength) {
							ViewModelStatus = new ViewModelStatus("您最多输入100个字", false, MessageType.Error, TipsType.DialogWithOkButton);
							return;
						}

						//TODO: 好像不到一千就报错
						if (Content.Length > Constants.MaxContentLength) {
							ViewModelStatus = new ViewModelStatus("您最多输入3000个字", false, MessageType.Error, TipsType.DialogWithOkButton);
							return;
						}

						if ((_viewType == NoticeViewType.GrowMemory) && (ImagesUrl == null || ImagesUrl.Count == 0)) {
							ViewModelStatus = new ViewModelStatus("请选择至少一张图片", false, MessageType.Error, TipsType.DialogWithOkButton);
							return;
						}

						//0. Sending Dialog Progress
						if (ImagesUrl != null && ImagesUrl.Count > 0) {
							ViewModelStatus = new ViewModelStatus("正在发送...图片发送较慢，请耐心等待偶...", true, MessageType.Information,
								TipsType.DialogProgress);
						} else {
							ViewModelStatus = new ViewModelStatus("正在发送...", true, MessageType.Information,
								TipsType.DialogProgress);
						}


						//Kindergarten Reicepe belong to Kindergarten All, so need transfer before send to api
//							_type = (_type==NoticeType.KindergartenRecipe) 
//								? NoticeType.KindergartenAll : _type;

						_tempNoticeSave = new NoticeModel {
							KindergartenId = BabyBusContext.KindergartenId,
							ClassId = BabyBusContext.ClassId,
							UserId = BabyBusContext.UserId,
							NoticeType = _type,
							Title = Title,
							Content = Content,
							ImageCount = 0,
							NormalPics = string.Empty,
						};

						if (ImagesUrl != null && ImagesUrl.Count > 0) {

							//
							_tempNoticeSave.ImageCount = ImagesUrl.Count;
							IList<UploadImageData> list = new List<UploadImageData>(_tempNoticeSave.ImageCount);
							for (var i = 0; i < _tempNoticeSave.ImageCount; i++) {
								string remoteFilename = Guid.NewGuid().ToString() + Constants.PNGSuffix;
								var data = new UploadImageData(false, remoteFilename, "还未开始上传", ImagesUrl[i]);
								list.Add(data);
							}
							try {
								await SendNoticeAndImage(list);
							} catch (OperationCanceledException ex) {
								ViewModelStatus = new ViewModelStatus("已经取消发送！", false, MessageType.Warning, TipsType.Undisplay);
								Mvx.Trace(ex.Message);
							} catch (Exception ex) {
								Mvx.Trace(ex.Message);
							}
                           
						} else {
							//除成长记忆以外都可以不发送图片
							try {
								await _remoteService.SendNotice(_tempNoticeSave);

								ViewModelStatus = new ViewModelStatus(UIConstants.SEND_SUCCESS, false, MessageType.Success);
                                        
								Redirect();

							} catch (Exception ex) {
								ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Error, TipsType.DialogWithOkButton);
							}
						}
					} finally {
					}
                  
				});
			}
		}

		//TODO: use show or goto as prefix, should review later
		public MvxCommand ShowChildrenPageCommand {
			get {
				_showChildrenPageCommand = _showChildrenPageCommand ??
				new MvxCommand(() => ShowViewModel<SelectChildrenViewModel>());
				return _showChildrenPageCommand;
			}
		}

		CancellationTokenSource _tokenSource = new CancellationTokenSource();

		private Task UploadImages(IList<UploadImageData> uploadImageDataList, SendImagesResultCallBack callback)
		{
			var _sw = new System.Diagnostics.Stopwatch();
			_sw.Reset();
			_sw.Start();
			// create the cancellation token
			CancellationToken token = _tokenSource.Token;

			return Task.Run(() => {
				IList<UploadImageData> successList = new List<UploadImageData>();
				IList<UploadImageData> failureList = new List<UploadImageData>();

				ImageHelper.InitOssClient();
				foreach (var uploadImageData in uploadImageDataList) {
					if (token.IsCancellationRequested) {
						throw new OperationCanceledException(token);
					}
					var result = ImageHelper.UploadImage(uploadImageData);
					if (result.IsSuccess) {
						successList.Add(uploadImageData);
					} else {
						//do nothing, user should dicide whether resend image or not
						failureList.Add(uploadImageData);
					}
				}

				_sw.Stop();
				long _elapsedMilliseconds = _sw.ElapsedMilliseconds;
				Debug.WriteLine(string.Format("Finished in {0} ms ({1:0.0} s total)", _sw.ElapsedMilliseconds,
					_elapsedMilliseconds / 1000.0));
                
				//call callback methods here
				callback(successList, failureList);
			}, token);
		}


		#endregion
	}
}