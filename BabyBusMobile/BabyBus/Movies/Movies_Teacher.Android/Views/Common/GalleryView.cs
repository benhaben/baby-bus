using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using BabyBus.Droid.Adapters;
using BabyBus.Droid.Views.Common.Album;
using Cirrious.MvvmCross.Droid.Views;

namespace BabyBus.Droid.Views.Common
{
    [Activity(Label = "GalleryView")]
    public class GalleryView : MvxActivity
    {
        private GalleryAdapter adapter;
        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

            RequestWindowFeature(WindowFeatures.NoTitle);
            // Create your application here
            SetContentView(Resource.Layout.Page_All_Gallery);
            Init();
        }

        private void Init() {
            var gallery = FindViewById<Gallery>(Resource.Id.gallery);
            adapter = new GalleryAdapter(this);
            gallery.Adapter = adapter;            
            gallery.RefreshDrawableState();

            var bt = FindViewById<Button>(Resource.Id.imagegrid_bt_delete);
            bt.Click += (sender, args) => {

                ImageCollection.DeleteImage();
                adapter.NotifyDataSetChanged();
            };
        }
    }
}