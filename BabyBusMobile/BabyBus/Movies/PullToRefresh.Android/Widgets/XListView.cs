using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Text;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Java.Lang;
using PullToRefresh.Android.Interfaces;
using String = System.String;

namespace PullToRefresh.Android.Widgets
{
    public class XListView : ListView, AbsListView.IOnScrollListener
    {
        private const int SCROLL_BACK_HEADER = 0;
        private const int SCROLL_BACK_FOOTER = 1;

        private const int SCROLL_DURATION = 400;

        // when pull up >= 50px
        private const int PULL_LOAD_MORE_DELTA = 50;

        // support iOS like pull
        private const float OFFSET_RADIO = 1.8f;

        private float mLastY = -1;

        // used for scroll back
        private Scroller mScroller;
        // user's scroll listener
        private IOnScrollListener mScrollListener;
        // for mScroller, scroll back from header or footer.
        private int mScrollBack;

        // the interface to trigger refresh and load more.
        private IXListViewListener mListener;

        private XHeaderView mHeader;
        // header view content, use it to calculate the Header's height. And hide it when disable pull refresh.
        private RelativeLayout mHeaderContent;
        private TextView mHeaderTime;
        private int mHeaderHeight;

        private LinearLayout mFooterLayout;
        private XFooterView mFooterView;
        private bool mIsFooterReady = false;

        private bool mEnablePullRefresh = true;
        private bool mPullRefreshing = false;

        private bool mEnablePullLoad = true;
        private bool mEnableAutoLoad = false;
        private bool mPullLoading = false;

        // total list items, used to detect is at the bottom of ListView
        private int mTotalItemCount;

        protected XListView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) {
        }

        public XListView(Context context) : base(context) {
            initWithContext(context);
        }

        public XListView(Context context, IAttributeSet attrs) : base(context, attrs) {
            initWithContext(context);
        }

        public XListView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle) {
            initWithContext(context);
        }

        private void initWithContext(Context context) {
            mScroller = new Scroller(context, new DecelerateInterpolator());
            base.SetOnScrollListener(this);

            // init header view
            mHeader = new XHeaderView(context);
            mHeaderContent = (RelativeLayout) mHeader.FindViewById(Resource.Id.header_content);
            mHeaderTime = (TextView) mHeader.FindViewById(Resource.Id.header_hint_time);
            AddHeaderView(mHeader);

            // init footer view
            mFooterView = new XFooterView(context);
            mFooterLayout = new LinearLayout(context);
            LinearLayout.LayoutParams lp = new LinearLayout.LayoutParams(LinearLayout
                .LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent);
            lp.Gravity = GravityFlags.Center;
            mFooterLayout.AddView(mFooterView, lp);


            // init header height
            GlobalLayoutListener listener = null;
            listener = new GlobalLayoutListener(new Action(() =>
            {
                mHeaderHeight = mHeaderContent.Height;
                mHeader.ViewTreeObserver.RemoveGlobalOnLayoutListener(listener);
            }));
            ViewTreeObserver observer = mHeader.ViewTreeObserver;
            if (null != observer)
            {
                observer.AddOnGlobalLayoutListener(listener);
            }
        }

        public override IListAdapter Adapter {
            get { return base.Adapter; }
            set {
                if (mIsFooterReady == false)
                {
                    mIsFooterReady = true;
                    AddFooterView(mFooterLayout);
                }
                base.Adapter = value;
            }
        }

        /**
     * Enable or disable pull down refresh feature.
     *
     * @param enable
     */

        public bool PullRefreshEnable {
            set {
                mHeaderContent.Visibility = value ? ViewStates.Visible : ViewStates.Invisible;
            }
        }

        /**
     * Enable or disable pull up load more feature.
     *
     * @param enable
     */
        public void setPullLoadEnable(bool enable) {
            mEnablePullLoad = enable;

//            if (!mEnablePullLoad)
//            {
//                mFooterView.setBottomMargin(0);
//                mFooterView.hide();
//                mFooterView.SetPadding(0, 0, 0, mFooterView.Height*(-1));
//                mFooterView.SetOnClickListener(null);
//
//            }
//            else
//            {
//                mPullLoading = false;
//                mFooterView.SetPadding(0, 0, 0, 0);
//                mFooterView.show();
//                mFooterView.setState(XFooterView.STATE_NORMAL);
//                // both "pull up" and "click" will invoke load more.
//                OnClickListener listener = null;
//                listener = new OnClickListener(() => {
//                    startLoadMore();
//                });
//
//                mFooterView.SetOnClickListener(listener);
//            }

            mPullLoading = false;
                            mFooterView.SetPadding(0, 0, 0, 0);
                            mFooterView.show();
                            mFooterView.setState(XFooterView.STATE_NORMAL);
                            // both "pull up" and "click" will invoke load more.
                            OnClickListener listener = null;
                            listener = new OnClickListener(() => {
                                startLoadMore();
                            });
            
                            mFooterView.SetOnClickListener(listener);
        }


        /**
     * Enable or disable auto load more feature when scroll to bottom.
     *
     * @param enable
     */
        public void setAutoLoadEnable(bool enable) {
            mEnableAutoLoad = enable;
        }

        /**
     * Stop refresh, reset header view.
     */

        public void stopRefresh() {
            if (mPullRefreshing)
            {
                mPullRefreshing = false;
                resetHeaderHeight();
            }
        }

        /**
     * Stop load more, reset footer view.
     */

        public void stopLoadMore(bool isEnd=false) {
            /*
            if (mPullLoading)
            {
                mPullLoading = false;
                if(isEnd)
                    mFooterView.setState(XFooterView.STATE_NOMORE);    
                else
                    mFooterView.setState(XFooterView.STATE_NORMAL);

            }*/
            mPullLoading = false;
            if (isEnd)
                mFooterView.setState(XFooterView.STATE_NOMORE);
            else
                mFooterView.setState(XFooterView.STATE_NORMAL);
        }
        /**
     * Set last refresh time
     *
     * @param time
     */

        public void setRefreshTime(String time) {
            mHeaderTime.Text = time;
        }

        /**
     * Set listener.
     *
     * @param listener
     */

        public void setXListViewListener(IXListViewListener listener) {
            mListener = listener;
        }

        /**
     * Auto call back refresh.
     */

        public void autoRefresh() {
            mHeader.setVisibleHeight(mHeaderHeight);

            if (mEnablePullRefresh && !mPullRefreshing)
            {
                // update the arrow image not refreshing
                if (mHeader.getVisibleHeight() > mHeaderHeight)
                {
                    mHeader.setState(XHeaderView.STATE_READY);
                }
                else
                {
                    mHeader.setState(XHeaderView.STATE_NORMAL);
                }
            }

            mPullRefreshing = true;
            mHeader.setState(XHeaderView.STATE_REFRESHING);
            refresh();
        }

        private void invokeOnScrolling() {
            if (mScrollListener is OnXScrollListener)
            {
                OnXScrollListener listener = (OnXScrollListener) mScrollListener;
                listener.onXScrolling(this);
            }
        }

        private void updateHeaderHeight(float delta) {
            mHeader.setVisibleHeight((int) delta + mHeader.getVisibleHeight());

            if (mEnablePullRefresh && !mPullRefreshing)
            {
                // update the arrow image unrefreshing
                if (mHeader.getVisibleHeight() > mHeaderHeight)
                {
                    mHeader.setState(XHeaderView.STATE_READY);
                }
                else
                {
                    mHeader.setState(XHeaderView.STATE_NORMAL);
                }
            }

            // scroll to top each time
            SetSelection(0);
        }

        private void resetHeaderHeight() {
            int height = mHeader.getVisibleHeight();
            if (height == 0) return;

            // refreshing and header isn't shown fully. do nothing.
            if (mPullRefreshing && height <= mHeaderHeight) return;

            // default: scroll back to dismiss header.
            int finalHeight = 0;
            // is refreshing, just scroll back to show all the header.
            if (mPullRefreshing && height > mHeaderHeight)
            {
                finalHeight = mHeaderHeight;
            }

            mScrollBack = SCROLL_BACK_HEADER;
            mScroller.StartScroll(0, height, 0, finalHeight - height, SCROLL_DURATION);

            // trigger computeScroll
            Invalidate();
        }

        private void updateFooterHeight(float delta) {
            int height = mFooterView.getBottomMargin() + (int) delta;

            if (mEnablePullLoad && !mPullLoading)
            {
                if (height > PULL_LOAD_MORE_DELTA)
                {
                    // height enough to invoke load more.
                    mFooterView.setState(XFooterView.STATE_READY);
                }
                else
                {
                    mFooterView.setState(XFooterView.STATE_NORMAL);
                }
            }

            mFooterView.setBottomMargin(height);

            // scroll to bottom
            // setSelection(mTotalItemCount - 1);
        }

        private void resetFooterHeight() {
            int bottomMargin = mFooterView.getBottomMargin();

            if (bottomMargin > 0)
            {
                mScrollBack = SCROLL_BACK_FOOTER;
                mScroller.StartScroll(0, bottomMargin, 0, -bottomMargin, SCROLL_DURATION);
                Invalidate();
            }
        }

        private void startLoadMore() {
            mPullLoading = true;
            mFooterView.setState(XFooterView.STATE_LOADING);
            loadMore();
        }

        public override bool OnTouchEvent(MotionEvent ev) {
            if (mLastY == -1)
            {
                mLastY = ev.RawY;
            }

            switch (ev.Action)
            {
                case MotionEventActions.Down:
                    mLastY = ev.RawY;
                    break;

                case MotionEventActions.Move:
                    float deltaY = ev.RawY - mLastY;
                    mLastY = ev.RawY;

                    if (FirstVisiblePosition == 0 && (mHeader.getVisibleHeight() > 0 ||
                                                           deltaY > 0))
                    {
                        // the first item is showing, header has shown or pull down.
                        updateHeaderHeight(deltaY/OFFSET_RADIO);
                        invokeOnScrolling();
                    }

                    else if (LastVisiblePosition == mTotalItemCount - 1 && (mFooterView
                        .getBottomMargin() > 0 || deltaY < 0))
                    {
                        // last item, already pulled up or want to pull up.
                        updateFooterHeight(-deltaY/OFFSET_RADIO);
                    }
                    break;

                default:
                    // reset
                    mLastY = -1;
                    if (FirstVisiblePosition == 0)
                    {
                        // invoke refresh
                        if (mEnablePullRefresh && mHeader.getVisibleHeight() > mHeaderHeight)
                        {
                            mPullRefreshing = true;
                            mHeader.setState(XHeaderView.STATE_REFRESHING);
                            refresh();
                        }

                        resetHeaderHeight();

                    }
                    else if (LastVisiblePosition == mTotalItemCount - 1)
                    {
                        // invoke load more.
                        if (mEnablePullLoad && mFooterView.getBottomMargin() > PULL_LOAD_MORE_DELTA)
                        {
                            startLoadMore();
                        }
                        resetFooterHeight();
                    }
                    break;
            }
            return base.OnTouchEvent(ev);
        }

        public override void ComputeScroll() {
            if (mScroller.ComputeScrollOffset())
            {
                if (mScrollBack == SCROLL_BACK_HEADER)
                {
                    mHeader.setVisibleHeight(mScroller.CurrY);
                }
                else
                {
                    mFooterView.setBottomMargin(mScroller.CurrY);
                }
                PostInvalidate();
                invokeOnScrolling();
            }

            base.ComputeScroll();
        }

        public override void SetOnScrollListener(IOnScrollListener l) {
            mScrollListener = l;
        }

        public void OnScrollStateChanged(AbsListView view, ScrollState scrollState) {
            if (mScrollListener != null)
            {
                mScrollListener.OnScrollStateChanged(view, scrollState);
            }
            if (scrollState == ScrollState.Idle)
            {
                if (mEnableAutoLoad && LastVisiblePosition == Count - 1)
                {
                    startLoadMore();
                }
            }
        }

        public void OnScroll(AbsListView view, int firstVisibleItem,
            int visibleItemCount, int totalItemCount) {
            // send to user's listener
            mTotalItemCount = totalItemCount;
            if (mScrollListener != null)
            {
                mScrollListener.OnScroll(view, firstVisibleItem, visibleItemCount, totalItemCount);
            }
        }

        private void refresh() {
            if (mEnablePullRefresh && null != mListener) {
                mListener.onRefresh();
            }
        }

        private void loadMore() {
            /*if (mEnablePullLoad && null != mListener)
            {
                mListener.onLoadMore();
            }*/
            if (null != mListener) {
                mListener.onLoadMore();
            }
        }

        /**
     * You can listen ListView.OnScrollListener or this one. it will invoke
     * onXScrolling when header/footer scroll back.
     */
        public interface OnXScrollListener : IOnScrollListener
        {
            void onXScrolling(View view);
        }

        public class GlobalLayoutListener : Java.Lang.Object, ViewTreeObserver.IOnGlobalLayoutListener
        {
            private readonly Action action;

            public GlobalLayoutListener(Action _action)
            {
                action = _action;
            }

            public void OnGlobalLayout()
            {
                if(action != null)
                    action.Invoke();
            }
        }

        public class OnClickListener : Java.Lang.Object, IOnClickListener
        {
            private readonly Action action;

            public OnClickListener(Action _action) {
                action = _action;
            }

            public void OnClick(View v) {
                if(action != null)
                    action.Invoke();
            }
        }
    }
}
