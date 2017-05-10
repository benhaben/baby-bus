using Android.OS;
using Exception = System.Exception;
using FileNotFoundException = Java.IO.FileNotFoundException;
using IOException = Java.IO.IOException;
using Math = System.Math;
using Uri = Android.Net.Uri;
using Android.Support.V4.App;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using BabyBus.Droid.Fragments;
using Cirrious.CrossCore;
using AndroidHUD;
using BabyBus.Logic.Shared;
using Com.Squareup.Picasso;


namespace BabyBus.Droid
{
	public class PhotoFragment : Fragment
	{
		private readonly Handler handler = new Handler();
		private readonly IDroidPictureService pic = Mvx.Resolve<IDroidPictureService>();
		private string _photoName;

		public static PhotoFragment NewInstance(string photoName)
		{
			var fragment = new PhotoFragment{ _photoName = photoName };
			return fragment;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var image = new ImageView(Activity) {
				LayoutParameters = new DrawerLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent
					, ViewGroup.LayoutParams.MatchParent)
			};
			image.SetScaleType(ImageView.ScaleType.FitCenter);
			if (!string.IsNullOrEmpty(_photoName)) {
				Picasso.With(Activity).Load(Constants.ThumbServerPath + _photoName).Priority(Picasso.Priority.High)
					.Placeholder(Resource.Drawable.icon_loading).Error(Resource.Drawable.icon_loading_error).Into(image);//.Resize(320, 125)
				image.LongClick += (sender, e) => {
					var p = new PhotoLongClickListener(Activity, image, _photoName);
					p.OnLongClick(image);
				};
			}

			//Zoom Magic Happen
//			var attacher = new PhotoViewAttacher(image);
//			
//			attacher.PhotoTap += (sender, args) => Activity.Finish();
//			if (!pic.CanLoadFromLocal(_photoName)) {
//				AndHUD.Shared.Show(Activity, "", -1, MaskType.Clear, TimeSpan.FromSeconds(50)
//					, null, true, () => AndHUD.Shared.Dismiss());
//			}
//			pic.LoadIamgeFromSource(_photoName, stream => {
//				AndHUD.Shared.Dismiss();
//				var ms = stream as MemoryStream;
//				if (ms != null) {
//					byte[] bytes = ms.ToArray();
//					var options = new BitmapFactory.Options { InPurgeable = true };
//					Bitmap bmp = BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length, options);
//					_bmp = bmp;
//					handler.Post(() => {
//						if (Activity != null && !Activity.IsFinishing) {
//							image.SetImageBitmap(bmp);
//							attacher.SetOnLongClickListener(new PhotoLongClickListener(Activity, image, _bmp));
//							attacher.Update();
//						}
//					});
//				}
//			}, message => handler.Post(() => {
//				if (Activity != null && !Activity.IsFinishing) {
//					AndHUD.Shared.ShowToast(Activity, message, MaskType.None, TimeSpan.FromSeconds(-1), true, () => AndHUD.Shared.Dismiss(Activity), () => AndHUD.Shared.Dismiss(Activity));
//				}
//			}));
				
			return image;
		}

		public override void OnPause()
		{
			base.OnPause();

			AndHUD.Shared.Dismiss();
		}


		public class PhotoLongClickListener : Java.Lang.Object, View.IOnLongClickListener
		{
			readonly FragmentActivity _context;
			ImageView _image;
			private readonly string _imageName;

			public PhotoLongClickListener(FragmentActivity context, ImageView image, string imageName)
			{
				_context = context;
				_image = image;
				_imageName = imageName;
			}

			public bool OnLongClick(View view)
			{
				var dialog = new PhotoOptionFragment(_imageName);
				dialog.Show(_context.SupportFragmentManager, "Dialog");

				return true;
			}
		}

	}
}

