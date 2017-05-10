using System;
using UIKit;
using BabyBus.iOS;

namespace BabyBus.iOS
{
    public class SelectBeginAndEndDateView:UIView
    {
        public SelectBeginAndEndDateView()
        {
        }

        SelectDateView _selectBeginDate;

        public SelectDateView SelectBeginDate
        {
            get
            {
                if (_selectBeginDate == null)
                {
                    _selectBeginDate = new SelectDateView();
                    _selectBeginDate.Title = "开始日期：";
                    _selectBeginDate.BackgroundColor = MvxTouchColor.White;
                  
                    _selectBeginDate.Layer.CornerRadius = EasyLayout.CornerRadius;
                    _selectBeginDate.Layer.MasksToBounds = true;
                    _selectBeginDate.UserInteractionEnabled = true;

                }
                return _selectBeginDate;
            }
        }

        SelectDateView _selectEndDate;

        public SelectDateView SelectEndDate
        {
            get
            {
                if (_selectEndDate == null)
                {
                    _selectEndDate = new SelectDateView();
                    _selectEndDate.Title = "结束日期：";
                    _selectEndDate.BackgroundColor = MvxTouchColor.White;
                    _selectEndDate.Layer.CornerRadius = EasyLayout.CornerRadius;
                    _selectEndDate.Layer.MasksToBounds = true;
                    _selectEndDate.UserInteractionEnabled = true;

                }
                return _selectEndDate;
            }
        }

        public DateTime BeginDate
        {
            get
            {
                var beginDate = (DateTime)SelectBeginDate.Date;
                return beginDate;
            }
        }

        public DateTime EndDate
        {
            get
            {
                var beginDate = (DateTime)SelectEndDate.Date;
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

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            UIView[] v =
                {
                    SelectBeginDate,
                    SelectEndDate,
                };
            Container.AddSubviews(v);
            AddSubviews(_container);
            SetUpConstrainLayout();
            base.LayoutSubviews();
        }

        public static nfloat SelectDateViewHeight = EasyLayout.NormalTextFieldHeight + EasyLayout.MarginMedium * 2;

        void SetUpConstrainLayout()
        {

            this.ConstrainLayout(
                () => 
                Container.Frame.Top == Frame.Top
                && Container.Frame.Left == Frame.Left
                && Container.Frame.Right == Frame.Right
                && Container.Frame.Bottom == Frame.Bottom

                && SelectBeginDate.Frame.Top == Container.Frame.Top + EasyLayout.MarginMedium
                && SelectBeginDate.Frame.Left == Container.Frame.Left
                && SelectBeginDate.Frame.Right == Container.Frame.GetCenterX()
                && SelectBeginDate.Frame.Height == SelectDateViewHeight

                && SelectEndDate.Frame.Top == SelectBeginDate.Frame.Top
                && SelectEndDate.Frame.Left == SelectBeginDate.Frame.Right
                && SelectEndDate.Frame.Right == Container.Frame.Right
                && SelectEndDate.Frame.Height == SelectDateViewHeight
            );
        }
    }
}

