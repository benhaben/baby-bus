using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;

namespace PullToRefresh.Android.Widgets
{
    public class XFooterView : LinearLayout
    {
        public const int STATE_NORMAL = 0;
    public const int STATE_READY = 1;
    public const int STATE_LOADING = 2;
    public const int STATE_NOMORE = 3;

    private const int ROTATE_ANIM_DURATION = 180;

    private View mLayout;

    private View mProgressBar;

    private TextView mHintView;

//    private ImageView mHintImage;

    private Animation mRotateUpAnim;
    private Animation mRotateDownAnim;

    private int mState = STATE_NORMAL;

    public XFooterView(Context context):base(context) {
        initView(context);
    }

    public XFooterView(Context context, IAttributeSet attrs) : base(context,attrs) {
        initView(context);
    }

    private void initView(Context context) {
        mLayout = LayoutInflater.From(context).Inflate(Resource.Layout.vw_footer, null);
        mLayout.LayoutParameters = new LinearLayout.LayoutParams(LayoutParams.MatchParent,
            LayoutParams.MatchParent);
        AddView(mLayout);

        mProgressBar = mLayout.FindViewById(Resource.Id.footer_progressbar);
        mHintView = (TextView) mLayout.FindViewById(Resource.Id.footer_hint_text);
//        mHintImage = (ImageView) mLayout.FindViewById(Resource.Id.footer_arrow);

        mRotateUpAnim = new RotateAnimation(0.0f, 180.0f, Dimension.RelativeToSelf, 0.5f,
                Dimension.RelativeToSelf, 0.5f);
        mRotateUpAnim.Duration = ROTATE_ANIM_DURATION;
        mRotateUpAnim.FillAfter = true;

        mRotateDownAnim = new RotateAnimation(180.0f, 0.0f, Dimension.RelativeToSelf, 0.5f,
                Dimension.RelativeToSelf, 0.5f);
        mRotateDownAnim.Duration = ROTATE_ANIM_DURATION;
        mRotateDownAnim.FillAfter = true;
    }

    /**
     * Set footer view state
     *
     * @see #STATE_LOADING
     * @see #STATE_NORMAL
     * @see #STATE_READY
     *
     * @param state
     */
    public void setState(int state) {
        if (state == mState) return;

        if (state == STATE_LOADING) {
//            mHintImage.clearAnimation();
//            mHintImage.setVisibility(View.INVISIBLE);
            mProgressBar.Visibility = ViewStates.Visible;
            mHintView.Visibility = ViewStates.Invisible;
        } else {
//            mHintImage.setVisibility(View.VISIBLE);
            mHintView.Visibility = ViewStates.Visible;
            mProgressBar.Visibility = ViewStates.Invisible;
        }

        switch (state) {
            case STATE_NORMAL:
//                if (mState == STATE_READY) {
//                    mHintImage.startAnimation(mRotateDownAnim);
//                }
//                if (mState == STATE_LOADING) {
//                    mHintImage.clearAnimation();
//                }
                mHintView.SetText(Resource.String.footer_hint_load_normal);
                break;

            case STATE_READY:
                if (mState != STATE_READY) {
//                    mHintImage.clearAnimation();
//                    mHintImage.startAnimation(mRotateUpAnim);
                    mHintView.SetText(Resource.String.footer_hint_load_ready);
                }
                break;

            case STATE_LOADING:
                break;
            case STATE_NOMORE:
                mHintView.SetText(Resource.String.footer_hint_load_nomore);
                break;
        }

        mState = state;
    }

    /**
     * Set footer view bottom margin.
     *
     * @param margin
     */
    public void setBottomMargin(int margin) {
        if (margin < 0) 
            return;
        LinearLayout.LayoutParams lp = (LinearLayout.LayoutParams) mLayout.LayoutParameters;
        lp.BottomMargin = margin;
        mLayout.LayoutParameters = lp;
    }

    /**
     * Get footer view bottom margin.
     *
     * @return
     */
    public int getBottomMargin() {
        LinearLayout.LayoutParams lp = (LinearLayout.LayoutParams) mLayout.LayoutParameters;
        return lp.BottomMargin;
    }

    /**
     * normal status
     */
    public void normal() {
        mHintView.Visibility = ViewStates.Visible;
        mProgressBar.Visibility = ViewStates.Gone;
    }

    /**
     * loading status
     */
    public void loading() {
        mHintView.Visibility = ViewStates.Gone;
        mProgressBar.Visibility = ViewStates.Visible;
    }

    /**
     * hide footer when disable pull load more
     */
    public void hide() {
        LinearLayout.LayoutParams lp = (LinearLayout.LayoutParams) mLayout.LayoutParameters;
        lp.Height = 0;
        mLayout.LayoutParameters = lp;

    }

    /**
     * show footer
     */

        public void show() {
            LinearLayout.LayoutParams lp = (LinearLayout.LayoutParams) mLayout.LayoutParameters;
            lp.Height = LayoutParams.WrapContent;
            mLayout.LayoutParameters = lp;
        }
    }
}
