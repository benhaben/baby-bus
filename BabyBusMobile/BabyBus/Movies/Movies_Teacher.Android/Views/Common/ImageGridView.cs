using System.IO;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using BabyBus.Droid.Adapters;
using BabyBus.Droid.Utils;
using BabyBus.Droid.Views.Common.Album;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.Plugins.PictureChooser;

namespace BabyBus.Droid.Views.Common
{
	/// <summary>
	/// Image Grid Album for Select.
	/// </summary>
    [Activity(Label = "ImageGridView")]
    public class ImageGridView : MvxActivity
    {
        private AlbumHelper helper;
        private int selectTotal;
        private IMvxPictureChooserTask pictureChooser;
        private ImageGridAdapter _adapter;

        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

            RequestWindowFeature(WindowFeatures.NoTitle);
            
            SetContentView(Resource.Layout.Page_All_ImageGrid);
            //Build Image List From Local File System
            helper = AlbumHelper.Instance;
            helper.Init(this);
            helper.BuildImageList();
            //Mvx Plugin
            pictureChooser = Mvx.Resolve<IMvxPictureChooserTask>();

            var bt = FindViewById<Button>(Resource.Id.imagegrid_bt_finish);
            //Bind Image List To GridView
            var gridview = FindViewById<GridView>(Resource.Id.imagegrid);
            _adapter = new ImageGridAdapter(this, helper.ImageList);
            gridview.Adapter = _adapter;

            gridview.ItemClick += (sender, args) => {
				ImageItem item = helper.ImageList[args.Position];
				if (ImageCollection.PthList.Count + selectTotal < CustomConfig.ImageMaxNumber || item.IsSelect)
                {
//                    if (args.Position == 0) {
//                        TakePicture();
//                        Finish();
//                        return;
//                    }
                    View view = args.View;
                    var isselect = view.FindViewById<ImageView>(Resource.Id.isselected);
                    var text = view.FindViewById<TextView>(Resource.Id.item_grid_image_text);
                    
                    item.IsSelect = !item.IsSelect;
					if (item.IsSelect)
					{
						isselect.SetImageResource(Resource.Drawable.icon_data_select);
                       	text.SetBackgroundResource(Resource.Drawable.bgd_relatly_line);
                       	selectTotal++;

                    }
                    else
                    {
						isselect.SetImageBitmap(null);
                       	text.SetBackgroundColor(new Color(0x00000000));
                        selectTotal--;
                    }
                    bt.Text = string.Format("完成({0}/{1})", selectTotal
                        , CustomConfig.ImageMaxNumber - ImageCollection.PthList.Count);
                }
            };

            bt.Click += (sender, args) => {
                helper.GetSelectedImageList();
                Finish();
            };
        }

        private void TakePicture() {
            pictureChooser.TakePicture(300, 100, OnPicture, () => { });
        }

        private void OnPicture(Stream picStream) {
            helper.GetTakedImage(picStream);
        }

        protected override void OnDestroy() {
            base.OnDestroy();
            
            _adapter.ClearCache();
        }
    }
}