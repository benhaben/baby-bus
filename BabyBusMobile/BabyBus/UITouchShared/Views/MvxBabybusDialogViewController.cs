using System;
using System.Diagnostics;
using BigTed;
using Cirrious.MvvmCross.Dialog.Touch;
using Cirrious.MvvmCross.Touch.Views;
using CrossUI.Touch.Dialog.Elements;
using SDWebImage;
using UIKit;
using BabyBus.Logic.Shared;

namespace BabyBus.iOS
{
    public class MvxBabybusDialogViewController :MvxDialogViewController
    {
       
        protected MvxBabybusDialogViewController(UITableViewStyle style = UITableViewStyle.Grouped,
                                                 RootElement root = null,
                                                 bool pushing = false)
            : base(style, root, pushing)
        {

            InitEvent();
        }

        public MvxBabybusDialogViewController(IntPtr handle)
            : base(handle)
        {
            InitEvent();
        }


        public virtual void OnViewDidLoad()
        {

            //must press long time to click row if have this method
            if (AddGestureWhenTap)
                HideKeyBoardWhenTap();

            var baseViewModel = ViewModel as BaseViewModel;

            if (baseViewModel != null)
            {
                if (NeedRegisterShowHUD)
                {
                    baseViewModel.InfoChanged += (object sender, EventArgs e) =>
                    {
                        InvokeOnMainThread(() => ShowHUD());
                    };
                }

                AfterViewDidLoadCalled += (sender, e) => baseViewModel.FirstLoad();
            }
            else
            {
                Debug.Assert(false);
            }

        }

        /// <summary>
        /// 使用事件让代码顺序执行，先初始化view，再调用FirstLoad，派生类使用OnViewDidLoad初始化数据
        /// </summary>
        void InitEvent()
        {
            ViewDidLoadCalled += (sender, e) => OnViewDidLoad();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            View.EndEditing(true);
        }

        bool _needRegisterShowHUD = true;

        public bool NeedRegisterShowHUD
        {
            get{ return _needRegisterShowHUD; }
            set{ _needRegisterShowHUD = value; }
        }

        //dialogView don't want this
        bool _addGestureWhenTap = false;

        public bool AddGestureWhenTap
        {
            get{ return _addGestureWhenTap; }
            set{ _addGestureWhenTap = value; }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }

        void HideKeyBoardWhenTap()
        {
            //TODO: test tap in the uitextview
            var g = new UITapGestureRecognizer(() =>
                {
                    var firstResponder = View.FindFirstResponder();
                    if (firstResponder != null)
                    {
                        firstResponder.ResignFirstResponder();
                    }
                });
            View.AddGestureRecognizer(g);
        }

        public virtual void ShowHUD()
        {
            this.InvokeOnMainThread(() =>
                {
                    var baseViewModel = ViewModel as BaseViewModel;
                    if (baseViewModel != null
                        && !string.IsNullOrEmpty(baseViewModel.Information)
                        && baseViewModel.ViewModelStatus.TipsType != TipsType.Undisplay)
                    {
                        //BTProgressHUD.Dismiss();
                   
                        if (baseViewModel.IsSuccessStatus)
                        {
                            BTProgressHUD.ShowSuccessWithStatus(baseViewModel.Information, Constants.RefreshTime);
                        }
                        else if (baseViewModel.IsErrorStatus)
                        {
                            BTProgressHUD.ShowErrorWithStatus(baseViewModel.Information, Constants.RefreshTime);
                        }
                        else if (baseViewModel.IsRunning)
                        {

                            ProgressHUD.Shared.Show(
                                "取消"
                            ,
                                () => ProgressHUD.Shared.ShowErrorWithStatus("取消操作!")
                            ,
                                baseViewModel.Information,
                                -1,
                                ProgressHUD.MaskType.None,
                                10000); 
                        }
                        else
                        {
                            BTProgressHUD.Dismiss();
                            BTProgressHUD.ShowToast(
                                baseViewModel.Information,
                                ProgressHUD.MaskType.None,
                                true,
                                Constants.RefreshTime);
                        }

                    }
                    else
                    {
                        BTProgressHUD.Dismiss();
                    }
                });
        }
    }
}

