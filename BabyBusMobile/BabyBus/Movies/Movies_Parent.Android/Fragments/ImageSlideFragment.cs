using System.IO;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using BabyBus.Droid.Views.Main;
using BabyBus.Services;
using Cirrious.CrossCore;

namespace BabyBus.Droid.Views.Frags {
    public class ImageSlideFragment : Fragment {
        private readonly Handler handler = new Handler();
        private readonly IPictureService pic = Mvx.Resolve<IPictureService>();
        private string _path;

        public static ImageSlideFragment NewInstance(string path) {
            var fragment = new ImageSlideFragment {_path = path};
            return fragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            var image = new ImageView(Activity) {
                LayoutParameters = new DrawerLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent
                    , ViewGroup.LayoutParams.MatchParent)
            };
            image.SetScaleType(ImageView.ScaleType.CenterCrop);
            pic.LoadIamgeFromSource(_path, stream => {
                var ms = stream as MemoryStream;
                if (ms != null) {
                    byte[] bytes = ms.ToArray();
                    var options = new BitmapFactory.Options {InPurgeable = true};
                    Bitmap bmp = BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length, options);
                    handler.Post(() => image.SetImageBitmap(bmp));
                }
            });

            image.Click += (sender, args) => {
                var parent = Activity.Parent as MainView;
                if (parent != null) {
                    parent.TabHost.SetCurrentTabByTag("GrowMemory");
                }
            };

            return image;
        }
    }
}