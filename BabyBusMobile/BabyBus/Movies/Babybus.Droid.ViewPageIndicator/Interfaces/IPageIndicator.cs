using Android.Support.V4.View;

namespace BabyBus.Droid.ViewPagerIndicator.Interfaces
{
    public interface IPageIndicator : ViewPager.IOnPageChangeListener
    {
        void SetViewPager(ViewPager view);
        void SetViewPager(ViewPager view, int initialPosition);
        int CurrentItem { get; set; }
        void SetOnPageChangeListener(ViewPager.IOnPageChangeListener listener);
        void NotifyDataSetChanged();
    }
}