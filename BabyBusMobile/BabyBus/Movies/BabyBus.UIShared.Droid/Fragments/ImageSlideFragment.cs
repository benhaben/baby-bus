using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using BabyBus.Logic.Shared;
using Com.Squareup.Picasso;
using Cirrious.MvvmCross.Droid.Fragging;

namespace BabyBus.Droid.Views.Frags
{
	public class ImageSlideFragment : Fragment
	{
		private int _drawableId = 0;
		private string _imageName = null;


		public static ImageSlideFragment NewInstance(int drawableId)
		{
			var fragment = new ImageSlideFragment { _drawableId = drawableId };
			return fragment;
		}

		public static ImageSlideFragment NewInstance(string imageName)
		{
			var fragment = new ImageSlideFragment { _imageName = imageName };
			return fragment;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var image = new ImageView(Activity) {
				LayoutParameters = new DrawerLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent
                    , ViewGroup.LayoutParams.MatchParent)
			};
			image.SetScaleType(ImageView.ScaleType.FitXy);
			if (_drawableId != 0) {
				Picasso.With(Activity).Load(_drawableId)
					.Placeholder(Resource.Drawable.bar_slide_1).Into(image);//.Resize(320, 125)
			} else if (!string.IsNullOrEmpty(_imageName)) {
				Picasso.With(Activity).Load(Constants.ThumbServerPath + _imageName)
					.Placeholder(Resource.Drawable.bar_slide_1).Into(image);//.Resize(320, 125)
			}
			return image;

		}
	}
}