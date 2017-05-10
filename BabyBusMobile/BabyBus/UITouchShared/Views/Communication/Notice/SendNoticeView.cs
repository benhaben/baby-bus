using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BigTed;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.BindingContext;
using CoreGraphics;
using ELCImagePickerControllerBinding;
using Foundation;
using MobileCoreServices;
using MWPhotoBrowserBinding;
using UIKit;
using BabyBus.Logic.Shared;
using BabyBusSSApi.ServiceModel.Enumeration;



namespace BabyBus.iOS
{
    public class MyUICollectionView:UICollectionView
    {
        public  MyUICollectionView(CGRect frame, UICollectionViewLayout layout, UIView theSuperView)
            : base(frame, layout)
        {
            _superView = theSuperView;
        }

        UIView _superView;

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);
            var firstResponder = _superView.FindFirstResponder();
            if (firstResponder != null)
            {
                firstResponder.ResignFirstResponder();
            }
            this.NextResponder.TouchesBegan(touches, evt);
        }
    }

    public partial class SendNoticeView
        : MvxBabyBusBaseAutoLayoutViewController,IELCImagePickerControllerDelegate
    {
        SendNoticeViewModel _baseViewModel = null;

        public SendNoticeView()
        {
            AddGestureWhenTap = false;
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        #region View lifecycle

        void CreateBinding()
        {
            var set = this.CreateBindingSet<SendNoticeView, SendNoticeViewModel>();
            set.Bind(NoticeTitle).To(vm => vm.Title);
            set.Bind(Content).To(vm => vm.Content);
            set.Apply();
        }

        void ConvertImageToBytes()
        {
            //Mote:if customer click cancel, and the image will duplicate

            //get progress percentage
            _delta = ImagesUrl.Count != 0 ? 1 / (float)ImagesUrl.Count : 0;
            _times = -1;
            _baseViewModel.ImagesUrl = this.ImagesUrl;
        }

        static int _times = -1;

        void ResetTimesAndProgress()
        {
            _times = -1;
            if (standardProgressView != null)
            {
                standardProgressView.RemoveFromSuperview();
            }
        }

        //TODO: don't work UIProgressView
        UIProgressView standardProgressView;
        float _delta = 0;

        void SendNotice()
        {
            ProgressHUD.Shared.Show("取消", () =>
                {
                    _baseViewModel.ClearDataAfterGiveUpSendNotice();
                    ResetTimesAndProgress();
                    ProgressHUD.Shared.ShowErrorWithStatus("取消操作!");
                }, "正在压缩图片...", -1, ProgressHUD.MaskType.None, 3000);
            this.NoticeTitle.ResignFirstResponder();
            this.Content.ResignFirstResponder();
            Task.Run(() =>
                {
                    this.InvokeOnMainThread(() =>
                        {
                            standardProgressView = new UIProgressView(UIProgressViewStyle.Bar);
                            standardProgressView.Frame = new CGRect(0, 0, View.Frame.Width, 10);
                            standardProgressView.AutoresizingMask = UIViewAutoresizing.FlexibleBottomMargin | UIViewAutoresizing.FlexibleTopMargin | UIViewAutoresizing.FlexibleWidth;
                            this.View.InsertSubview(standardProgressView, 0);
                            ConvertImageToBytes();
                        });
                }).ContinueWith(t =>
                {
                    this.InvokeOnMainThread(() =>
                        {
                            if (t.Exception == null)
                            {
                                _baseViewModel.SendCommand.Execute();
                                this.DismissViewController(false, null);
                            }
                            else
                            {
                                BTProgressHUD.ShowToast(string.Format("压缩图片失败，请稍后重试。"), ProgressHUD.MaskType.Gradient, true, Constants.RefreshTime);
                            }
                        });
                });
        }

        public override void OnViewDidLoad()
        {
            base.OnViewDidLoad();

            _baseViewModel = ViewModel as SendNoticeViewModel;

            _baseViewModel.ImageHelper.SendImageProgressResultEventHandler += (UploadImageData uploadImageData) =>
            {
                this.InvokeOnMainThread(() =>
                    {
                        MvxTrace.Trace(MvxTraceLevel.Diagnostic, "SendImageProgressResultEventHandler occur");
                        if (_times == -1)
                        {
                            _times++;
                            standardProgressView.Progress = 0;
                        }
                        _times++;
                        standardProgressView.Progress += (float)_delta;
                        BTProgressHUD.ShowToast(string.Format("正在发送第{0}张图片。", _times), ProgressHUD.MaskType.Gradient, true, Constants.RefreshTime);
                    });
            };

            _baseViewModel.SendImagesResultEventHandler 
                += (IList<UploadImageData> successlist, IList<UploadImageData> failureList) => this.InvokeOnMainThread(() =>
                {

                    ResetTimesAndProgress();

                    if (failureList.Count > 0)
                    {
                        var result = string.Format("上传成功{0}张图片，失败{1}张图片。", successlist.Count, failureList.Count);
                        var alert = new UIAlertView("错误", result, null, "放弃发送", new String []{ "重新发送" });
                        alert.Clicked += (object sender, UIButtonEventArgs e) =>
                        {
                            var str = string.Format("UIAlertView click button {0}th", e.ButtonIndex);
                            MvxTrace.Trace(MvxTraceLevel.Diagnostic, str);
                            if (e.ButtonIndex == 0)
                            {
                                //do nothing
                                ProgressHUD.Shared.Dismiss();
                                _baseViewModel.ClearDataAfterGiveUpSendNotice();
                            }
                            else
                            {
                                //resend images
                                _baseViewModel.SendNoticeAndImage(failureList);
                            }
                        };
                        alert.AlertViewStyle = UIAlertViewStyle.Default;
                        alert.Canceled += (object sender, EventArgs e) =>
                        {
                            _baseViewModel.ClearDataAfterGiveUpSendNotice();
                            ResetTimesAndProgress();
                            MvxTrace.Trace(MvxTraceLevel.Diagnostic, "Canceled UIAlertView 放弃发送");
                        };
                        alert.Show();
                    }
                });

            if (_baseViewModel.NoticeType == NoticeType.GrowMemory)
            {
                this.NoticeTitle.Placeholder = "请写下这一刻的想法";
            }
                
            _doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, (sender, args) =>
                {
                    var alert = new UIAlertView("提示", "您确定要发送吗，别忘了检查错别字哦？", null, "发送", new String []{ "回到编辑" });
                    alert.Clicked += (object s, UIButtonEventArgs e) =>
                    {
                        if (e.ButtonIndex == 0)
                        {
                            SendNotice(); 
                        }
                        else
                        {
                        }
                    };
                    alert.AlertViewStyle = UIAlertViewStyle.Default;
                    alert.Show();
                });
            this.NavigationItem.SetRightBarButtonItem(_doneButton, true);
            this.NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (sender1, args1) =>
                    {
                        var alert = new UIAlertView("提示", "您确定要放弃编辑吗？", null, "放弃编辑", new String []{ "回到编辑" });
                        alert.Clicked += (object sender, UIButtonEventArgs e) =>
                        {
                            if (e.ButtonIndex == 0)
                            {
                                _baseViewModel.Close();
                            }
                            else
                            {
                            }
                        };
                        alert.AlertViewStyle = UIAlertViewStyle.Default;
                        alert.Show();
                    }), true);

            // Perform any additional setup after loading the view, typically from a nib.
            CreateBinding();

            var inset = ScrollView.ContentInset;
            var alignmentRectInsets = ScrollView.AlignmentRectInsets;
           
            CollectionView.Source = NoticeImageDataSource;
            CollectionView.ReloadData();

            var label = new UILabel(new CGRect(0, 0, 100, 35));
            if (_baseViewModel.NoticeType == NoticeType.GrowMemory)
            {
                label.Text = "传照片";
            }
            else if (_baseViewModel.NoticeType == NoticeType.KindergartenStaff)
            {
                label.Text = "园务通知";
            }
            else if (_baseViewModel.NoticeType == NoticeType.KindergartenAll)
            {
                label.Text = "园区通知";
            }
            else if (_baseViewModel.NoticeType == NoticeType.ClassHomework)
            {
                label.Text = "留作业";
            }
            else if (_baseViewModel.NoticeType == NoticeType.ClassEmergency)
            {
                label.Text = "孩子考勤";
            }
            else if (_baseViewModel.NoticeType == NoticeType.ClassCommon)
            {
                label.Text = "发通知";
            }

            this.NavigationItem.TitleView = label;
            label.TextAlignment = UITextAlignment.Center;
            label.TextColor = MvxTouchColor.White;

            Content.Changed += (sender, e) =>
            {
                nfloat heightChanged = Content.SizeThatFits(new CGSize(280, float.MaxValue)).Height;
                _heightConstrainContent.Constant = _heightConstrainContent.Constant > heightChanged ? _heightConstrainContent.Constant : heightChanged;
            };
        }

        UIBarButtonItem _doneButton;

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            if (_baseViewModel.NoticeType == NoticeType.GrowMemory)
            {
                _heightConstrainContent.Constant = 0;
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
        }

        #endregion

        SendNoticeImageDataSource _noticeImageDataSource = null;

        public SendNoticeImageDataSource NoticeImageDataSource
        {
            get
            {
                if (_noticeImageDataSource == null)
                {
                    _noticeImageDataSource = new SendNoticeImageDataSource(this);
                    _noticeImageDataSource.ImageViewSize = new CGSize(70, 70);
                    _noticeImageDataSource.SetImages(new List<string>{ "images/ic_adpho_1.png" });
                }
                return _noticeImageDataSource;
            }
            set
            {
                _noticeImageDataSource = value;
            }
        }

        MyUICollectionView _collectionView = null;

        public MyUICollectionView CollectionView
        {
            get
            {
                if (_collectionView == null)
                {
                    // Flow Layout
                    var flowLayout = new UICollectionViewFlowLayout()
                    {
                        HeaderReferenceSize = new CGSize(0, 0),
                        SectionInset = new UIEdgeInsets(5, 10, 5, 10),
                        ScrollDirection = UICollectionViewScrollDirection.Vertical,
                        MinimumInteritemSpacing = 4, // minimum spacing between cells
                        MinimumLineSpacing = 4, // minimum spacing between rows if ScrollDirection is Vertical or between columns if Horizontal
                        ItemSize = new CGSize(70, 70)
                    };
        
                    //frame is setted in the SetUpConstrainLayout
                    _collectionView = new MyUICollectionView(new CGRect(0, 0, 0, 0), flowLayout, View);
                    _collectionView.ContentInset = new UIEdgeInsets(1, 1, 1, 1);
                    _collectionView.RegisterClassForCell(typeof(MWImageCollecntionViewCell), MWImageCollecntionViewCell.CellID);
                    _collectionView.ShowsHorizontalScrollIndicator = false;

//                    _collectionView.Layer.BorderWidth = EasyLayout.BorderWidth;
//                    _collectionView.Layer.BorderColor = MvxTouchColor.Gray3.CGColor;
//                    _collectionView.Layer.CornerRadius = EasyLayout.CornerRadius;
//                    _collectionView.Layer.MasksToBounds = true;
                    _collectionView.BackgroundColor = MvxTouchColor.White;
                }
                return _collectionView;
            }
        }


        List<string> _imagesUrl = null;

        public List<string> ImagesUrl
        {
            get
            { 
                if (_imagesUrl == null)
                {
                    _imagesUrl = new List<string>();
                }
                return _imagesUrl;
            }
            set
            { 
                _imagesUrl = value;
            }
        }


        InsetsTextField _title = null;

        public InsetsTextField NoticeTitle
        {
            get
            {
                if (_title == null)
                {   
                    _title = new InsetsTextField(new UIEdgeInsets(10, 10, 10, 10));
                    _title.Placeholder = "标题";
                    _title.BackgroundColor = MvxTouchColor.White;
                    _title.TextColor = MvxTouchColor.Gray1;
                    _title.Font = EasyLayout.ContentFont;
                }
                return _title;
            }
        }

        UITextView _content = null;

        public UITextView Content
        {
            get
            {
                if (_content == null)
                {
                    _content = new UITextView();
                    _content.Font = EasyLayout.ContentFont;
                    _content.BackgroundColor = MvxTouchColor.White;
                    _content.TextColor = MvxTouchColor.Gray1;
                    _content.TextContainerInset = new UIEdgeInsets(10, 10, 10, 10);
                }
                return _content;
            }
        }

       
        public override void PrepareViewHierarchy()
        {
            base.PrepareViewHierarchy();
           
            UIView[] v =
                {
                    NoticeTitle,
                    Content,
                    CollectionView
                };

            Container.AddSubviews(v);
            #if __DEBUDUI__
            Container.BackgroundColor = UIColor.Red;
            ScrollView.BackgroundColor = UIColor.Blue;
            View.BackgroundColor = UIColor.Green;
            #endif
        }

        protected override void SetBackgroundImage()
        {
            base.SetBackgroundImage();
            this.View.BackgroundColor = MvxTouchColor.White2;
        }

        public override void SetUpConstrainLayout()
        {
            base.SetUpConstrainLayout();
            nfloat oneP = 0f;
            View.ConstrainLayout 
            (
                // Analysis disable CompareOfFloatsByEqualityOperator
                () => 
                NoticeTitle.Frame.Height == EasyLayout.NormalTextViewHeight
                && NoticeTitle.Frame.Left == Container.Frame.Left
                && NoticeTitle.Frame.Right == Container.Frame.Right
                && NoticeTitle.Frame.Top == Container.Frame.Top + oneP

                && Content.Frame.Left == Container.Frame.Left
                && Content.Frame.Right == Container.Frame.Right
                && Content.Frame.Top == NoticeTitle.Frame.Bottom + EasyLayout.MarginMedium

                && CollectionView.Frame.Left == Container.Frame.Left
                && CollectionView.Frame.Right == Container.Frame.Right
                && CollectionView.Frame.Top == Content.Frame.Bottom + EasyLayout.MarginMedium

                && Container.Frame.Bottom == CollectionView.Frame.Bottom

                // Analysis restore CompareOfFloatsByEqualityOperator
            );

            //Note: do not change frame, change Constrain to resize control
            var constrains =
                View.ConstrainLayout(
                    () => Content.Frame.Height == EasyLayout.HeightOfContent
                    && CollectionView.Frame.Height == EasyLayout.HeightOfSelectImageCollection
                );
            _heightConstrainContent = constrains[0];
            _heightConstrainCollectionView = constrains[1];
            _heightConstrainCollectionView.Constant += 3;
        }

        NSLayoutConstraint _heightConstrainContent;
        NSLayoutConstraint _heightConstrainCollectionView;

        void IELCImagePickerControllerDelegate.ECLImagePickerController(ELCImagePickerController picker, NSObject[] info)
        {

//            UIImagePickerControllerMediaType = ALAssetTypePhoto; UIImagePickerControllerReferenceURL = "assets-library://asset/asset.JPG?id=6E5438ED-9A8C-4ED0-9DEA-AB2D8F8A9360&ext=JPG";
            foreach (NSDictionary dict in info)
            {
                var imageUrl = dict.ValueForKey(new NSString("UIImagePickerControllerReferenceURL")).ToString();
                if (ImagesUrl.Count < EasyLayout.MaximumImagesCount)
                {
                    ImagesUrl.Add(imageUrl);
                }
                else
                {
                    BTProgressHUD.ShowToast(string.Format("您最多选择{0}张图片。", EasyLayout.MaximumImagesCount), ProgressHUD.MaskType.Gradient, true, Constants.RefreshTime);
                }
            }
            this.NoticeImageDataSource.InsertIntoHead(ImagesUrl);
            this.CollectionView.ReloadData();
            this.DismissViewController(true, null);
        }

        void IELCImagePickerControllerDelegate.ECLImagePickerControllerDidCancel(ELCImagePickerController picker)
        {
            this.DismissViewController(true, null);
        }
    }
    //class

    public class SendNoticeImageDataSource : UICollectionViewSource
    {
        UIViewController _vc;

        public SendNoticeImageDataSource(UIViewController vc)
        {
            Images = new List<string>();
            _vc = vc;
        }

        /// <summary>
        /// Gets or sets the images. improve: here use big picuter, outside use thumb png
        /// </summary>
        /// <value>The images.</value>
        private List<string> Images { get; set; }

        public int Count{ get { return  Images.Count; } }

        public void SetImages(List<string> list)
        {
            Images.Clear();
            Images = list;
        }

        public void InsertIntoHead(List<string> list)
        {
            Images.RemoveRange(0, Images.Count - 1);
            Images.InsertRange(0, list);
        }

        public CGSize ImageViewSize { get; set; }

        public override nint NumberOfSections(UICollectionView collectionView)
        {
            return 1;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return Images.Count;
        }

        public override bool ShouldSelectItem(UICollectionView collectionView, NSIndexPath indexPath)
        {
            if (indexPath.Row == Images.Count - 1)
            {
                ELCImagePickerController elcPicker = new ELCImagePickerController();
                elcPicker.MaximumImagesCount = EasyLayout.MaximumImagesCount;
                elcPicker.ReturnsOriginalImage = false;
                elcPicker.ReturnsImage = false;
                elcPicker.OnOrder = true;
                elcPicker.MediaTypes = new string[]
                {
                    UTType.Image
                };
                elcPicker.WeakELCImagePickerControllerDelegate = _vc;
                _vc.PresentViewController(elcPicker, true, null);
            }
            else
            {
                //show photo browser

                var imageList = Images;
                var firstImage = imageList[indexPath.Row];
                imageList.RemoveAt(indexPath.Row);
                imageList.Insert(0, firstImage);

                int number = imageList.Count - 1;
                _select.Clear();
                _mwPhotos.Clear();

                for (int i = 0; i < number; i++)
                {
                    var photo = MWPhoto.PhotoWithURL(NSUrl.FromString(imageList[i]));
                    photo.Caption = "";
                    _mwPhotos.Add(photo);
                    _select.Add(true);
                }

                MWPhotoBrowser browser = new MWPhotoBrowser();
                browser.DisplayActionButton = false;
                browser.DisplayNavArrows = true;
                browser.DisplaySelectionButtons = true;
                browser.AlwaysShowControls = true;
                browser.ZoomPhotosToFill = true;
                browser.EnableGrid = false;
                browser.StartOnGrid = false;
                browser.EnableSwipeToDismiss = true;
                browser.NumberOfPhotosInPhotoBrowser = NumberOfPhotosInPhotoBrowser;
                browser.IsPhotoSelectedAtIndex = IsPhotoSelectedAtIndex;
                browser.PhotoAtIndex = PhotoAtIndex;
                browser.PhotoBrowserDidFinishModalPresentation += (object sender, EventArgs e) =>
                {
                    //do image
                    List<string> addImages = new List<string>();

                    for (int i = 0; i < _select.Count; i++)
                    {
                        if (_select[i])
                        {
                            addImages.Add(Images[i]);
                        }
                        else
                        {
                        }
                    }
                    //last image is +
                    Images.RemoveRange(0, Images.Count - 1);
                    Images.InsertRange(0, addImages);
                    collectionView.ReloadData();

                    var snv = _vc as SendNoticeView;
                    snv.ImagesUrl = addImages;
                    // If we subscribe to this method we must dismiss the view controller ourselves
                    _vc.DismissViewController(true, null);
                };
                browser.PhotoAtIndexSelectedChanged += (object sender, PhotoAtIndexSelectedChangedEventArgs e) =>
                {
                    _select[(int)e.Index] = e.Selected;
                };
                browser.DidDisplayPhotoAtIndex += (object sender, DidDisplayPhotoAtIndexEventArgs e) =>
                {
                    
                };
                // Modal
                UINavigationController nc = new UINavigationController(browser);
                nc.ModalTransitionStyle = UIModalTransitionStyle.CrossDissolve;
                _vc.PresentViewController(nc, true, null);
            }
            return true;
        }

        List<MWPhoto> _mwPhotos = new List<MWPhoto>();
        List<bool> _select = new List<bool>();

        bool IsPhotoSelectedAtIndex(MWPhotoBrowser photoBrowser, uint index)
        {
            return _select[(int)index];
        }

        public  MWPhoto PhotoAtIndex(MWPhotoBrowser photoBrowser, uint index)
        {
            return _mwPhotos[(int)index];
        }

        int NumberOfPhotosInPhotoBrowser(MWPhotoBrowser photoBrowser)
        {
            return  Images.Count - 1;
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = (MWImageCollecntionViewCell)collectionView.DequeueReusableCell(MWImageCollecntionViewCell.CellID, indexPath);
             
            if (indexPath.Row >= 0 && indexPath.Row < Images.Count)
            {
                string url = Images[indexPath.Row];
                MWPhoto photo = null;
                if (indexPath.Row == Images.Count - 1)
                {
                    var addImage = UIImage.FromBundle(url);
                    photo = MWPhoto.PhotoWithImage(addImage);
                }
                else
                {
                    photo = MWPhoto.PhotoWithURL(new NSUrl(url));
                }

                if (photo != null)
                {
                    photo.LoadUnderlyingImageAndNotify();
                    cell.SetPhoto(photo);
                }
            }

            return cell;
        }
    }
}
//namespace

