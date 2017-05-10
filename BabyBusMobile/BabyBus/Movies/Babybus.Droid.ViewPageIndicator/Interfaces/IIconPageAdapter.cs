namespace BabyBus.Droid.ViewPagerIndicator.Interfaces
{
    public interface IIconPageAdapter
    {
        int GetIconResId(int index);
        int Count { get; }
    }
}