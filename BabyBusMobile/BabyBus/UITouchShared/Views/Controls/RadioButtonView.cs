using System;
using UIKit;
using BabyBus.iOS;
using CoreGraphics;
using System.Collections.Generic;

namespace BabyBus.iOS
{
    public class RadioButtonView:UIView
    {
        UILabel _titleLabel = null;

        public UILabel TitleLabel
        {
            get
            {
                if (_titleLabel == null)
                {
                    _titleLabel = new UILabel();
                    _titleLabel.BackgroundColor = MvxTouchColor.White;
                    _titleLabel.Font = EasyLayout.ContentFont;
                    _titleLabel.TextColor = MvxTouchColor.Gray1;
                    _titleLabel.TextAlignment = UITextAlignment.Center;
                }
                return _titleLabel;
            }
        }

        public string Title
        {
            get
            {
                return TitleLabel.Text;
            }
            set
            {
                TitleLabel.Text = value;
            }
        }

        UIButton _button1 = null;

        public UIButton Button1
        {
            get
            {
                if (_button1 == null)
                {
                    _button1 = new UIButton();
                    _button1.BackgroundColor = MvxTouchColor.Gray1;
                    _button1.Layer.CornerRadius = 4;
                    _button1.Layer.MasksToBounds = false;
                    _button1.ContentMode = UIViewContentMode.ScaleAspectFill;
                    _button1.ClipsToBounds = true;

                    _button1.Font = EasyLayout.SmallFont;
                }
                return _button1;
            }
        }

        UIButton _button2 = null;

        public UIButton Button2
        {
            get
            {
                if (_button2 == null)
                {
                    _button2 = new UIButton();
                    _button2.BackgroundColor = MvxTouchColor.Gray1;
                    _button2.Layer.CornerRadius = 4;
                    _button2.Layer.MasksToBounds = false;
                    _button2.ContentMode = UIViewContentMode.ScaleAspectFill;
                    _button2.ClipsToBounds = true;

                    _button2.Font = EasyLayout.SmallFont;

                }
                return _button2;

            }
        }

        UIButton _button3 = null;

        public UIButton Button3
        {
            get
            {
                if (_button3 == null)
                {
                    _button3 = new UIButton();
                    _button3.BackgroundColor = MvxTouchColor.Gray1;
                    _button3.Layer.CornerRadius = 4;
                    _button3.Layer.MasksToBounds = false;
                    _button3.ContentMode = UIViewContentMode.ScaleAspectFill;
                    _button3.ClipsToBounds = true;

                    _button3.Font = EasyLayout.SmallFont;

                }
                return _button3;
            }
        }


        UIButton _button4 = null;

        public UIButton Button4
        {
            get
            {
                if (_button4 == null)
                {
                    _button4 = new UIButton();
                    _button4.BackgroundColor = MvxTouchColor.Gray1;
                    _button4.Layer.CornerRadius = 4;
                    _button4.Layer.MasksToBounds = false;
                    _button4.ContentMode = UIViewContentMode.ScaleAspectFill;
                    _button4.ClipsToBounds = true;

                    _button4.Font = EasyLayout.SmallFont;

                }
                return _button4;
            }
        }

        UIButton _button5 = null;

        public UIButton Button5
        {
            get
            {
                if (_button5 == null)
                {
                    _button5 = new UIButton();
                    _button5.BackgroundColor = MvxTouchColor.Gray1;
                    _button5.Layer.CornerRadius = 4;
                    _button5.Layer.MasksToBounds = false;
                    _button5.ContentMode = UIViewContentMode.ScaleAspectFill;
                    _button5.ClipsToBounds = true;
                    _button5.Font = EasyLayout.SmallFont;

                }
                return _button5;
            }
        }

        UIButton _button6 = null;

        public UIButton Button6
        {
            get
            {
                if (_button6 == null)
                {
                    _button6 = new UIButton();
                    _button6.BackgroundColor = MvxTouchColor.Gray1;
                    _button6.Layer.CornerRadius = 4;
                    _button6.Layer.MasksToBounds = false;
                    _button6.ContentMode = UIViewContentMode.ScaleAspectFill;
                    _button6.ClipsToBounds = true;

                    _button6.Font = EasyLayout.SmallFont;

                }
                return _button6;
            }
        }

        public List<UIButton>  ButtonList
        {
            get;
            set;
        }

        public override UIView HitTest(CoreGraphics.CGPoint point, UIEvent uievent)
        {
            var pointView = this.ConvertPointFromView(point, this);
            if (this.PointInside(pointView, uievent))
            {
                int i = 0;
                foreach (var btn in ButtonList)
                {
                    CGPoint btnPointInA = btn.ConvertPointFromView(point, this);
                    if (btn.PointInside(btnPointInA, uievent))
                    {
                        btn.BackgroundColor = MvxTouchColor.Blue;
                        if (TouchButton != null)
                        {
                            TouchButton(btn, i);
                        }
                    }
                    else
                    {
                        btn.BackgroundColor = MvxTouchColor.Gray1;
                    }
                    i++;
                }
            }

            // 否则，返回默认处理
            return base.HitTest(point, uievent);
        }

        public event EventHandler<int> TouchButton;

        public RadioButtonView()
        {
            ButtonList = new List<UIButton>{ Button1, Button2, Button3, Button4, Button5, Button6 };
        }

        public void SetButtonsName(List<string> names)
        {
            if (names == null)
            {
                return;
            }

            for (var i = 0; i < names.Count; i++)
            {
                ButtonList[i].SetTitle(names[i], UIControlState.Normal);
            }
        }

        UIView _container = null;

        public UIView Container
        {
            get
            {
                if (_container == null)
                {
                    _container = new UIView();
                    _container.BackgroundColor = MvxTouchColor.White;
                }
                return _container;
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            UIView[] v =
                {
                    TitleLabel,
                    Button1,
                    Button2,
                    Button3,
                    Button4,
                    Button5,
                    Button6,

                };
            Container.AddSubviews(v);
            AddSubviews(_container);
            SetUpConstrainLayout();
            base.LayoutSubviews();
        }

        void SetUpConstrainLayout()
        {

            nfloat avgSpace = 320 / 3;
            nfloat buttonWidth = 60;
            nfloat buttonHeight = 20;

            nfloat button1Left = avgSpace / 2 - buttonWidth / 2;
            nfloat button2Left = avgSpace + avgSpace / 2 - buttonWidth / 2;
            nfloat button3Left = avgSpace * 2 + avgSpace / 2 - buttonWidth / 2;

            this.ConstrainLayout(
                () => 
                Container.Frame.Top == Frame.Top
                && Container.Frame.Left == Frame.Left
                && Container.Frame.Right == Frame.Right
                && Container.Frame.Bottom == Frame.Bottom

                && TitleLabel.Frame.Top == Container.Frame.Top + EasyLayout.MarginMedium
                && TitleLabel.Frame.Left == Container.Frame.Left
                && TitleLabel.Frame.Right == Container.Frame.Right

                && Button1.Frame.Top == TitleLabel.Frame.Bottom + EasyLayout.MarginMedium
                && Button1.Frame.Left == Container.Frame.Left + button1Left
                && Button1.Frame.Height == buttonHeight
                && Button1.Frame.Width == buttonWidth

                && Button2.Frame.Top == Button1.Frame.Top
                && Button2.Frame.Left == Container.Frame.Left + button2Left
                && Button2.Frame.Height == buttonHeight
                && Button2.Frame.Width == buttonWidth

                && Button3.Frame.Top == Button2.Frame.Top
                && Button3.Frame.Left == Container.Frame.Left + button3Left
                && Button3.Frame.Height == buttonHeight
                && Button3.Frame.Width == buttonWidth

                && Button4.Frame.Top == Button1.Frame.Bottom + EasyLayout.MarginMedium
                && Button4.Frame.Left == Button1.Frame.Left
                && Button4.Frame.Height == buttonHeight
                && Button4.Frame.Width == buttonWidth

                && Button5.Frame.Top == Button4.Frame.Top
                && Button5.Frame.Left == Button2.Frame.Left
                && Button5.Frame.Height == buttonHeight
                && Button5.Frame.Width == buttonWidth

                && Button6.Frame.Top == Button5.Frame.Top
                && Button6.Frame.Left == Button3.Frame.Left
                && Button6.Frame.Height == buttonHeight
                && Button6.Frame.Width == buttonWidth
            );
        }
    }
}

