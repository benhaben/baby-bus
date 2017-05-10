using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Bluetooth;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;

namespace PullToRefresh.Android.Widgets
{
    public class XHeaderView : LinearLayout
    {
    public const int STATE_NORMAL = 0;
    public const int STATE_READY = 1;
    public const int STATE_REFRESHING = 2;

    private int ROTATE_ANIM_DURATION = 180;

    private LinearLayout mContainer;

    private ImageView mArrowImageView;

    private ProgressBar mProgressBar;

    private TextView mHintTextView;

    private int mState = STATE_NORMAL;

    private Animation mRotateUpAnim;
    private Animation mRotateDownAnim;

    private bool mIsFirst;

    public XHeaderView(Context context):base(context) {
        initView(context);
    }

    public XHeaderView(Context context, IAttributeSet attrs):base(context,attrs) {
        initView(context);
    }

    private void initView(Context context) {
        // Initial set header view height 0
        LinearLayout.LayoutParams lp = new LinearLayout.LayoutParams(LayoutParams.MatchParent, 0);
        mContainer = (LinearLayout) LayoutInflater.From(context).Inflate(Resource.Layout.vw_header, null);
        AddView(mContainer, lp);
        SetGravity(GravityFlags.Bottom);

        mArrowImageView = (ImageView) FindViewById(Resource.Id.header_arrow);
        mHintTextView = (TextView) FindViewById(Resource.Id.header_hint_text);
        mProgressBar = (ProgressBar) FindViewById(Resource.Id.header_progressbar);

        mRotateUpAnim = new RotateAnimation(0.0f, -180.0f, Dimension.RelativeToSelf, 0.5f,
                Dimension.RelativeToSelf, 0.5f);
        mRotateUpAnim.Duration = ROTATE_ANIM_DURATION;
        mRotateUpAnim.FillAfter = true;


        mRotateDownAnim = new RotateAnimation(-180.0f, 0.0f, Dimension.RelativeToSelf, 0.5f,
                Dimension.RelativeToSelf, 0.5f);
        mRotateDownAnim.Duration = ROTATE_ANIM_DURATION;
        mRotateDownAnim.FillAfter = true;
    }

        public int State {
            set {
            if (value == mState && mIsFirst) {
                mIsFirst = true;
                return;
            }

            if (value == STATE_REFRESHING) {
                // show progress
                mArrowImageView.ClearAnimation();
                mArrowImageView.Visibility = ViewStates.Invisible;
                mProgressBar.Visibility = ViewStates.Visible;

            }
            else {
                // show arrow image
                mArrowImageView.Visibility = ViewStates.Visible;
                mProgressBar.Visibility = ViewStates.Invisible;
            }

            switch (value) {
                case STATE_NORMAL:
                    if (mState == STATE_READY) {
                        mArrowImageView.StartAnimation(mRotateDownAnim);
                    }

                    if (mState == STATE_REFRESHING) {
                        mArrowImageView.ClearAnimation();
                    }

                    mHintTextView.SetText(Resource.String.header_hint_refresh_normal);
                    break;

                case STATE_READY:
                    if (mState != STATE_READY) {
                        mArrowImageView.ClearAnimation();
                        mArrowImageView.StartAnimation(mRotateUpAnim);
                        mHintTextView.SetText(Resource.String.header_hint_refresh_ready);
                    }
                    break;

                case STATE_REFRESHING:
                    mHintTextView.SetText(Resource.String.header_hint_refresh_loading);
                    break;

                default:
                    break;
            }

            mState = value;
            }
        }

    public void setState(int state) {
        if (state == mState && mIsFirst) {
            mIsFirst = true;
            return;
        }

        if (state == STATE_REFRESHING) {
            // show progress
            mArrowImageView.ClearAnimation();
            mArrowImageView.Visibility = ViewStates.Invisible;
            mProgressBar.Visibility = ViewStates.Visible;

        } else {
            // show arrow image
            mArrowImageView.Visibility = ViewStates.Visible;
            mProgressBar.Visibility = ViewStates.Invisible;
        }

        switch (state) {
            case STATE_NORMAL:
                if (mState == STATE_READY) {
                    mArrowImageView.StartAnimation(mRotateDownAnim);
                }

                if (mState == STATE_REFRESHING) {
                    mArrowImageView.ClearAnimation();
                }

                mHintTextView.SetText(Resource.String.header_hint_refresh_normal);
                break;

            case STATE_READY:
                if (mState != STATE_READY) {
                    mArrowImageView.ClearAnimation();
                    mArrowImageView.StartAnimation(mRotateUpAnim);
                    mHintTextView.SetText(Resource.String.header_hint_refresh_ready);
                }
                break;

            case STATE_REFRESHING:
                mHintTextView.SetText(Resource.String.header_hint_refresh_loading);
                break;

            default:
                break;
        }

        mState = state;
    }

        public int VisibleHeight {
            get {
                return mContainer.Height;
            }
            set {
            if (value < 0)
                value = 0;
            LinearLayout.LayoutParams lp = (LinearLayout.LayoutParams)mContainer.LayoutParameters;
            lp.Height = value;
            mContainer.LayoutParameters = lp;
            }
        }

    /**
     * Set the header view visible height.
     *
     * @param height
     */
    public void setVisibleHeight(int height) {
        if (height < 0) 
            height = 0;
        LinearLayout.LayoutParams lp = (LinearLayout.LayoutParams) mContainer.LayoutParameters;
        lp.Height = height;
        mContainer.LayoutParameters = lp;
    }

    /**
     * Get the header view visible height.
     *
     * @return
     */
    public int getVisibleHeight() {
        return mContainer.Height;
    }
    }
}
