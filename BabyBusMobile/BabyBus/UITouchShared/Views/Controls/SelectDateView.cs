using System;
using UIKit;
using BabyBus.iOS;
using Foundation;

namespace BabyBus.iOS
{

    public class NoCopyUITextField:UITextField
    {
        public NoCopyUITextField()
        {
        }


        public NoCopyUITextField(CoreGraphics.CGRect frame)
            : base(frame)
        {
        }

        public override bool CanPerform(ObjCRuntime.Selector action, NSObject withSender)
        {
            UIMenuController menuController = UIMenuController.SharedMenuController;
            if (menuController != null)
            {
                UIMenuController.SharedMenuController.MenuVisible = false;
            }
            return false;
        }
    }

    public class SelectDateView:UIView
    {
        public SelectDateView()
        {
        }

        UILabel _label = null;

        public UILabel Label
        {
            get
            {
                if (_label == null)
                {
                    _label = new UILabel();
                    _label.BackgroundColor = MvxTouchColor.White;
                    _label.TextColor = MvxTouchColor.Gray1;
                    _label.Font = EasyLayout.SmallFont;
                    _label.UserInteractionEnabled = true;
                }
                return _label;
            }
        }

        const string DateFormate = "MMM-d";
        NoCopyUITextField _selectnDate;

        public NoCopyUITextField SelectDate
        {
            get
            {
                if (_selectnDate == null)
                {
                    _selectnDate = new NoCopyUITextField();
                    _selectnDate.BackgroundColor = MvxTouchColor.White;
//                    _selectBeginDate.Font = EasyLayout.TitleFont;
//                    _selectBeginDate.Layer.BorderColor = MvxTouchColor.Gray3.CGColor;
//                    _selectBeginDate.Layer.BorderWidth = EasyLayout.BorderWidth;
//                    _selectBeginDate.Layer.CornerRadius = EasyLayout.CornerRadius;
//                    _selectBeginDate.Layer.MasksToBounds = true;
                    _selectnDate.InputView = PickDate;
                    _selectnDate.UserInteractionEnabled = true;
                    _selectnDate.Text = DateTime.Now.Date.ToString(DateFormate);

                    _selectnDate.TextColor = MvxTouchColor.Gray1;
                    _selectnDate.Font = EasyLayout.SmallFont;
                    SelectDate.ValueChanged += (object sender, EventArgs e) =>
                    {
                    };
                }
                return _selectnDate;
            }
        }

        UIDatePicker _pickDate = null;

        public UIDatePicker PickDate
        {
            get
            {
                if (_pickDate == null)
                {
                    _pickDate = new UIDatePicker();
                    _pickDate.BackgroundColor = MvxTouchColor.White;
                    _pickDate.Mode = UIDatePickerMode.Date;
                    _pickDate.UserInteractionEnabled = true;
//                    var maxDate = DateTime.Now;
//                    maxDate = maxDate.AddMonths(1);
//                    _pickDate.MaximumDate = (NSDate)maxDate;
//                    var minDate = DateTime.Now;
//                    minDate = maxDate.AddMonths(-1);
//                    _pickDate.MinimumDate = (NSDate)minDate;
                    _pickDate.ValueChanged += (object sender, EventArgs e) =>
                    {
                        var beginDate = (DateTime)_pickDate.Date;
                        SelectDate.Text = beginDate.ToString(DateFormate);
                    };
                }
                return _pickDate;
            }
        }

        public DateTime Date
        {
            get
            {
                var beginDate = (DateTime)PickDate.Date;
                return beginDate;
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

        public string Title
        {
            get
            {
                return Label.Text;
            }
            set
            {
                Label.Text = value;
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            UIView[] v =
                {
                    Label,
                    SelectDate,
                };
            Container.AddSubviews(v);
            AddSubviews(_container);
            SetUpConstrainLayout();
            base.LayoutSubviews();
        }

        void SetUpConstrainLayout()
        {

            nfloat LabelWidth = 70;
            nfloat LabelHeight = EasyLayout.NormalTextFieldHeight;

            this.ConstrainLayout(
                () => 
                Container.Frame.Top == Frame.Top
                && Container.Frame.Left == Frame.Left
                && Container.Frame.Right == Frame.Right
                && Container.Frame.Bottom == Frame.Bottom

                && Label.Frame.Top == Container.Frame.Top + EasyLayout.MarginMedium
                && Label.Frame.Left == Container.Frame.Left + EasyLayout.MarginMedium
                && Label.Frame.Height == LabelHeight
                && Label.Frame.Width == LabelWidth

                && SelectDate.Frame.GetCenterY() == Label.Frame.GetCenterY()
                && SelectDate.Frame.Left == Label.Frame.Right + EasyLayout.MarginMedium
                && SelectDate.Frame.Height == LabelHeight
                && SelectDate.Frame.Right == Container.Frame.Right - EasyLayout.MarginMedium
            );
        }
    }
}

