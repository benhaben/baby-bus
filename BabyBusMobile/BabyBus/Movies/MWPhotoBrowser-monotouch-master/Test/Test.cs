using System;
using System.Collections.Generic;
using Foundation;
using MWPhotoBrowserBinding;
using UIKit;

namespace Tests {
    public class TestPhotoBrowser: UIViewController {
        public TestPhotoBrowser() {
        }

        UIButton _button = null;

        public UIButton Button {
            get {
                if (_button == null) {
                    _button = new UIButton(UIButtonType.System);
                    _button.Frame = new CoreGraphics.CGRect(20, 200, 100, 30);
                    _button.SetTitle("测试", UIControlState.Normal);
                }
                return _button;
            }
        }

        List<MWPhoto> _photos = new List<MWPhoto>();
        List<MWPhoto> _thumbs = new List<MWPhoto>();
        List<bool> _selections = new List<bool>();

        public override void ViewDidLoad() {
            base.ViewDidLoad();
            this.View.AddSubview(Button);
            this.View.BackgroundColor = UIColor.White;
            Button.TouchUpInside += (sender, e) => {
                //                this.photos = photos;
                //                this.thumbs = thumbs;

//                var photo = MWPhoto.PhotoWithImage(UIImage.FromBundle("ad-1"));
//                photo.Caption = "yin";
//                _photos.Add(photo);
//                _thumbs.Add(MWPhoto.PhotoWithImage(UIImage.FromBundle("ad-1")));
//
//                photo = MWPhoto.PhotoWithImage(UIImage.FromBundle("ad-2"));
//                photo.Caption = "yin";
//                _photos.Add(photo);
//                _thumbs.Add(MWPhoto.PhotoWithImage(UIImage.FromBundle("ad-2")));
//
//                photo = MWPhoto.PhotoWithImage(UIImage.FromBundle("ad-3"));
//                photo.Caption = "yin";
//                _photos.Add(photo);
//                _thumbs.Add(MWPhoto.PhotoWithImage(UIImage.FromBundle("ad-3")));

                //                photo = MWPhoto.PhotoWithURL(NSUrl.FromString("ad-2.png"));
                //                photo.Caption = "Central Atrium in Casa Batlló";
                //                _photos.Add(photo);
                //                _thumbs.Add(MWPhoto.PhotoWithURL(NSUrl.FromString("ad-2.png")));
                //
                //          
//                <img src="http://photo.yupoo.com/tousheng/CuZVLveX/medish.jpg" width="440" height="440" alt="遇到吸尘器的时候。。。" class="photo">
                var photo = MWPhoto.PhotoWithURL(NSUrl.FromString("http://photo.yupoo.com/tousheng/CuZVLveX/medish.jpg"));
                photo.Caption = "The Royal Albert Hall";
                _photos.Add(photo);
                _thumbs.Add(MWPhoto.PhotoWithURL(NSUrl.FromString("http://photo.yupoo.com/tousheng/CuZVLveX/medish.jpg")));
                         
                photo = MWPhoto.PhotoWithURL(NSUrl.FromString("http://photo.yupoo.com/tousheng/CuZVLveX/medish.jpg"));
                photo.Caption = "The ";
                _photos.Add(photo);
                _thumbs.Add(MWPhoto.PhotoWithURL(NSUrl.FromString("http://photo.yupoo.com/tousheng/CuZVLveX/medish.jpg")));

                photo = MWPhoto.PhotoWithURL(NSUrl.FromString("http://photo.yupoo.com/tousheng/CuZVLveX/medish.jpg"));
                photo.Caption = "The Royal A";
                _photos.Add(photo);
                _thumbs.Add(MWPhoto.PhotoWithURL(NSUrl.FromString("http://photo.yupoo.com/tousheng/CuZVLveX/medish.jpg")));

                photo = MWPhoto.PhotoWithURL(NSUrl.FromString("http://photo.yupoo.com/tousheng/CuZVLveX/medish.jpg"));
                photo.Caption = "The Royal sasdas Hall";
                _photos.Add(photo);
                _thumbs.Add(MWPhoto.PhotoWithURL(NSUrl.FromString("http://photo.yupoo.com/tousheng/CuZVLveX/medish.jpg")));

//                NSObject[] array = new NSObject[_photos.Count];
//                for (int i = 0; i < _photos.Count; i++) {
//                    array[i] = _photos[i];
//                }

                // Create browser
                MWPhotoBrowser browser = new MWPhotoBrowser();
                browser.DisplayActionButton = true;
                browser.DisplayNavArrows = true;
                browser.DisplaySelectionButtons = true;
                browser.AlwaysShowControls = true;
                browser.ZoomPhotosToFill = true;
                browser.EnableGrid = false;
                browser.StartOnGrid = true;
                browser.EnableSwipeToDismiss = true;

                browser.CurrentPhotoIndex = (0);
                browser.PhotoAtIndex = PhotoAtIndex;
                browser.ThumbPhotoAtIndex = ThumbPhotoAtIndex;
                browser.CaptionViewForPhotoAtIndex = CaptionViewForPhotoAtIndex;
                browser.TitleForPhotoAtIndex = TitleForPhotoAtIndex;
                browser.IsPhotoSelectedAtIndex = IsPhotoSelectedAtIndex;
                browser.PhotoAtIndexSelectedChanged += PhotoAtIndexSelectedChanged;
                browser.NumberOfPhotosInPhotoBrowser = NumberOfPhotosInPhotoBrowser;
                browser.PhotoBrowserDidFinishModalPresentation += PhotoBrowserDidFinishModalPresentation;
//                browser.ActionButtonPressedForPhotoAtIndex = null;

                // Reset selections
                for (int i = 0; i < _photos.Count; i++) {
                    _selections.Add(true);
                }

                // Push
                this.NavigationController.PushViewController(browser, true);
            };
        }

        #region IMWPhotoBrowserDelegate implementation

        public virtual MWPhoto PhotoAtIndex(MWPhotoBrowser photoBrowser, uint index) {
            if (index < _photos.Count)
                return _photos[(int)index];
            return null;
        }

        public MWPhoto ThumbPhotoAtIndex(MWPhotoBrowser photoBrowser, uint index) {
            if (index < _thumbs.Count)
                return _thumbs[(int)index];
            return null;
        }

        public MWCaptionView CaptionViewForPhotoAtIndex(MWPhotoBrowser photoBrowser, uint index) {
            MWPhoto photo = _photos[(int)index];
            MWCaptionView captionView = new MWCaptionView(photo);
            return captionView;
        }

        public string TitleForPhotoAtIndex(MWPhotoBrowser photoBrowser, uint index) {
            MWPhoto photo = _photos[(int)index];
            return photo.Caption;
        }
        
        //        public void DidDisplayPhotoAtIndex(MWPhotoBrowser photoBrowser, uint index) {
        //        }
        //
        //        public void ActionButtonPressedForPhotoAtIndex(MWPhotoBrowser photoBrowser, uint index) {
        //        }
        
        public bool IsPhotoSelectedAtIndex(MWPhotoBrowser photoBrowser, uint index) {
            return _selections[(int)index];
        }

        public void PhotoAtIndexSelectedChanged(object photoBrowser, PhotoAtIndexSelectedChangedEventArgs e) {
            _selections[(int)e.Index] = e.Selected;
        }

        public virtual int  NumberOfPhotosInPhotoBrowser(MWPhotoBrowser photoBrowser) {
            return _photos.Count;
        }

        
        public virtual  void  PhotoAtIndexSelectedChanged(MWPhotoBrowser photoBrowser, int index, bool selected) {
            _selections[index] = selected;
        }

        public virtual  void PhotoBrowserDidFinishModalPresentation(object sender, EventArgs e) {
            this.DismissViewController(true, null);
        }

        #endregion
    }
}

