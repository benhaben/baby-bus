using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace BabyBus.Droid.Control {
    public class SegmentedGroup : RadioGroup {
        private int oneDP;
        private Color mTintColor;
        private int mCheckedTextColor = Color.White;

        protected SegmentedGroup(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) {
        }

        public SegmentedGroup(Context context) : base(context) {
            mTintColor = Resources.GetColor(Resource.Color.radio_button_selected_color);
            oneDP = (int) TypedValue.ApplyDimension(ComplexUnitType.Dip, 1, Resources.DisplayMetrics);

        }

        public SegmentedGroup(Context context, IAttributeSet attrs) : base(context, attrs) {
            mTintColor = Resources.GetColor(Resource.Color.radio_button_selected_color);
            oneDP = (int) TypedValue.ApplyDimension(ComplexUnitType.Dip, 1, Resources.DisplayMetrics);
        }



        protected override void OnFinishInflate() {
            base.OnFinishInflate();
            //Use holo light for default
            UpdateBackground();
        }

        public void SetTintColor(Color tintColor) {
            mTintColor = tintColor;
            UpdateBackground();
        }

        public void SetTintColor(int tintColor) {
            mTintColor = Resources.GetColor(Resource.Color.bb_blue);
            UpdateBackground();
        }

        public void SetTintColor(Color tintColor, Color checkedTextColor) {
            mTintColor = tintColor;
            mCheckedTextColor = checkedTextColor;
            UpdateBackground();
        }

        public void UpdateBackground() {
            var count = ChildCount;
            if (count > 1) {
                var child = GetChildAt(0);
                var initParams = (LayoutParams) child.LayoutParameters;
                var newParams = new LayoutParams(initParams.Width, initParams.Height, initParams.Weight);
                newParams.SetMargins(0, 0, (int) (-oneDP), 0);
                child.LayoutParameters = newParams;
                UpdateBackground(GetChildAt(0), Resource.Drawable.radio_checked_left,
                    Resource.Drawable.radio_unchecked_left);
                for (int i = 1; i < count - 1; i++) {
                    UpdateBackground(GetChildAt(i), Resource.Drawable.radio_checked_middle,
                        Resource.Drawable.radio_unchecked_middle);
                    var child2 = GetChildAt(i);
                    initParams = (LayoutParams) child2.LayoutParameters;
                    newParams = new LayoutParams(initParams.Width, initParams.Height, initParams.Weight);
                    newParams.SetMargins(0, 0, (int) -oneDP, 0);
                    child2.LayoutParameters = newParams;
                }
                UpdateBackground(GetChildAt(count - 1), Resource.Drawable.radio_checked_right,
                    Resource.Drawable.radio_unchecked_right);
            }
            else if (count == 1) {
                UpdateBackground(GetChildAt(0), Resource.Drawable.radio_checked_default,
                    Resource.Drawable.radio_unchecked_default);
            }
        }

        private void UpdateBackground(View view, int check, int uncheck) {
            //Set text color
            ColorStateList colorStateList = new ColorStateList(new int[][] {
                new int[] {Android.Resource.Attribute.StatePressed},
                new int[] {-Android.Resource.Attribute.StatePressed, -Android.Resource.Attribute.StateChecked},
                new int[] {-Android.Resource.Attribute.StatePressed, Android.Resource.Attribute.StateChecked}
            },
                new int[] {Color.Gray, mTintColor, mCheckedTextColor});
            ((Button) view).SetTextColor(colorStateList);

            //Redraw with tint color
            Drawable checkedDrawable = Resources.GetDrawable(check).Mutate();
            Drawable uncheckedDrawable = Resources.GetDrawable(uncheck).Mutate();
            ((GradientDrawable) checkedDrawable).SetColor(mTintColor);
            ((GradientDrawable) uncheckedDrawable).SetStroke(oneDP, mTintColor);

            //Create drawable
            StateListDrawable stateListDrawable = new StateListDrawable();
            stateListDrawable.AddState(new int[] {-Android.Resource.Attribute.StateChecked}, uncheckedDrawable);
            stateListDrawable.AddState(new int[] {Android.Resource.Attribute.StateChecked}, checkedDrawable);

            //Set button background
            view.SetBackgroundDrawable(stateListDrawable);
        }
    }
}