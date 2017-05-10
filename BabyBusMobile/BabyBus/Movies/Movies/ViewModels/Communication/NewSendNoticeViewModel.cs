using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyBus.Models;
using BabyBus.Models.Communication;
using BabyBus.Models.Enums;
using BabyBus.Net.Communication;
using BabyBus.Services;
using BabyBus.Utilities;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using Newtonsoft.Json;
using Cirrious.CrossCore.Platform;

namespace BabyBus.ViewModels.Communication {
    public class NewSendNoticeViewModel : BaseViewModel {
        private readonly INoticeService noticeService;

        private NoticeType _type;
        private NoticeViewType _viewType;
        private string selectedText = "全班";

        public NewSendNoticeViewModel() {
            noticeService = Mvx.Resolve<INoticeService>();
            var messenger = Mvx.Resolve<IMvxMessenger>();

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

        public event EventHandler ClearImageCollection;

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


        public void Init(int type) {
            _type = (NoticeType)type;
        }

        public override void Start() {
            base.Start();

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
            } else {
                Title = string.Empty;
            }
        }

        #region Property

        private List<byte[]> _bytesList = new List<byte[]>();
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
            get { return contentHolder; }
            set { contentHolder = value; }
        }

        /// <summary>
        ///     Image Bytes List
        /// </summary>
        public List<byte[]> BytesList {
            get { return _bytesList; }
            set { _bytesList = value; }
        }

        #endregion

        private void GeneratorImagesNameIntoSendData(IList<UploadImageData> list) {
            if (_tempNoticeSave != null && list != null && list.Count > 0) {

                //如果以前有部分名字，加上一个，在末尾
                if (!string.IsNullOrWhiteSpace(_tempNoticeSave.NormalPics)) {
                    _tempNoticeSave.NormalPics += ",";
                }

                //拼接字符串，用，分割
                foreach (var data in list) {
                    _tempNoticeSave.NormalPics += data.FileName;
                    _tempNoticeSave.NormalPics += ",";
                }

                //去掉一个，在末尾
                _tempNoticeSave.NormalPics = _tempNoticeSave.NormalPics.Remove(_tempNoticeSave.NormalPics.Length - 1);
            }
        }

        #region Command

        public void SendNoticeAndImage(IList<UploadImageData> list) {
            UploadImages(list, (successList, failureList) => Task.Run(async () => {
                if (failureList.Count == 0 && successList.Count > 0) {
                    try {
                        //Note: should use += here
                        GeneratorImagesNameIntoSendData(successList);
                        ApiResponser result = await noticeService.SendNotice(_tempNoticeSave);
                        if (result.Status) {
                            _tempNoticeSave = JsonConvert.DeserializeObject<NoticeModel>(result.Attach.ToString());
                            ViewModelStatus = new ViewModelStatus("发送成功", false, MessageType.Success);
                            Close(this);
                        } else {
                            ViewModelStatus = new ViewModelStatus(result.Message, false, MessageType.Error, TipsType.DialogWithOkButton);
                        }
                    } catch (Exception ex) {
                        ViewModelStatus = new ViewModelStatus(ex.Message, false, MessageType.Error, TipsType.DialogWithOkButton);

                        //TODO: report to Xamarin insight
                        MvxTrace.Trace(MvxTraceLevel.Error, ex.Message);
                    }
                } else {
                    //a part of image upload success, so save them
                    GeneratorImagesNameIntoSendData(successList);
                }


                //call back no matter success or failure
                if (SendImagesResultEventHandler != null) {
                    SendImagesResultEventHandler(successList, failureList);
                }
            }));
        }

        /// <summary>
        /// when send notice, we keep the noticemodel, if send image fail, we can reuse the notice and send again
        /// </summary>
        private NoticeModel _tempNoticeSave = null;

        public void ClearDataAfterGiveUpSendNotice() {
            _tempNoticeSave = null;
            //when fill list when click done, so do not need to keep it
            BytesList.Clear();
        }

        private MvxCommand _showChildrenPageCommand;

        public MvxCommand SendCommand {
            get {
                return new MvxCommand(async () => {
                    //Check
                    if (_viewType == NoticeViewType.GrowMemory && (BytesList == null || BytesList.Count == 0)) {
                        ViewModelStatus = new ViewModelStatus("请选择至少一张图片");
                    }
                    if (string.IsNullOrEmpty(Title)) {
                        ViewModelStatus = new ViewModelStatus("请输入标题");
                        return;
                    }
                    if (_viewType != NoticeViewType.GrowMemory
                        && string.IsNullOrEmpty(Content)) {
                        ViewModelStatus = new ViewModelStatus("请输入内容");
                        return;
                    }
                    if (Content.Length > Constants.MaxContentLength) {
                        ViewModelStatus = new ViewModelStatus("您最多输入1000个字");
                        return;
                    }
                    if (_viewType != NoticeViewType.GrowMemory
                        && string.IsNullOrEmpty(Content.Trim())) {
                        ViewModelStatus = new ViewModelStatus("内容不能为空格");
                        return;
                    }

                    ViewModelStatus = new ViewModelStatus("正在发送...", true, MessageType.Information,
                        TipsType.DialogProgress);

                    _tempNoticeSave = new NoticeModel {
                        KindergartenId = BabyBusContext.KindergartenId,
                        ClassId = BabyBusContext.ClassId,
                        UserId = BabyBusContext.UserId,
                        NoticeType = _type,
                        Title = Title,
                        Content = Content,
                        ImageCount = 0,
                    };

                    if (BytesList != null && BytesList.Count > 0) {
                        _tempNoticeSave.ImageCount = BytesList.Count;
                        IList<UploadImageData> list = new List<UploadImageData>(_tempNoticeSave.ImageCount);
                        for (var i = 0; i < _tempNoticeSave.ImageCount; i++) {
                            string filename = Guid.NewGuid().ToString() + Constants.PNGSuffix;
                            var data = new UploadImageData(false, filename, "还未开始上传", BytesList[i]);
                            list.Add(data);
                        }
                        SendNoticeAndImage(list);
                    } else {
                        //除成长记忆以外都可以不发送图片
                        try {
                            ApiResponser result = await noticeService.SendNotice(_tempNoticeSave);
                            if (result.Status) {
                                ViewModelStatus = new ViewModelStatus("发送成功");
                                Close(this);
                            } else {
                                ViewModelStatus = new ViewModelStatus(result.Message, false, MessageType.Error, TipsType.DialogWithOkButton);
                            }
                        } catch (Exception ex) {
                            ViewModelStatus = new ViewModelStatus(ex.Message, false, MessageType.Error, TipsType.DialogWithOkButton);
                            Debug.WriteLine(ex.Message);
                        }
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

        private void UploadImages(IList<UploadImageData> uploadImageDataList, SendImagesResultCallBack callback) {
            Task.Run(() => {
                IList<UploadImageData> successList = new List<UploadImageData>();
                IList<UploadImageData> failureList = new List<UploadImageData>();

                foreach (var uploadImageData in uploadImageDataList) {
                    var result = ImageHelper.UploadImage(uploadImageData);
                    if (result.IsSuccess) {
                        successList.Add(uploadImageData);
                    } else {
                        //do nothing, user should dicide whether resend image or not
                        failureList.Add(uploadImageData);
                    }
                }

                //call callback methods here
                callback(successList, failureList);

                ClearImageCollection(null, null);
            });
        }

        #endregion
    }
}