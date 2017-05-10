using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BabyBus.Droid.Services;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using BabyBus.Logic.Shared;
using System.IO;
using Com.Squareup.Picasso;
using System.Threading.Tasks;

namespace BabyBus.Droid.Fragments
{
	public class PhotoOptionFragment:Android.Support.V4.App.DialogFragment
	{
		private string _imageName;

		public PhotoOptionFragment(string  imageName)
		{
			_imageName = imageName;
		}

		public override Dialog OnCreateDialog(Bundle savedInstanceState)
		{
			var list = new ListView(Activity);
			list.Adapter = new ArrayAdapter(Activity, Resource.Layout.Item_TextView, GetData());

			var dialog = new AlertDialog.Builder(Activity);
			dialog.SetTitle("保存到本地");
			//dialog.SetView(list);
			dialog.SetPositiveButton("确认", (s, a) => {
//				var pic = new PictureService();
//				Bitmap bmp = new Bitmap();
//				Task task = Task.Factory.StartNew(() => {
//					bmp = Picasso.With(Activity).Load(_imageName).Get();
//					return bmp;
//
//				});
//				task.Wait();
//				var result = pic.SaveImageInAlbum(Activity, bmp);
//				pic.LoadIamgeFromSource(_imageName, stream => {
					
//					var ms = stream as MemoryStream;
//					if (ms != null) {
//						byte[] bytes = ms.ToArray();
//						var options = new BitmapFactory.Options { InPurgeable = true };
//						Bitmap bmp = BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length, options);
//						var result = pic.SaveImageInAlbum(Activity, bmp);
//					}
//				}, Constants.ThumbServerPath);
				//Picasso picasso = new Picasso.Builder();
				var target = new PicassoTarget(Activity, _imageName);
				Picasso.With(Activity).Load(Constants.ThumbServerPath + _imageName).Into(target);

			});
			dialog.SetNegativeButton("取消", (s, a) => {
			});

//            list.ItemClick += (sender, args) => {
//                if (args.Position == 0) {
//                    var pic = new PictureService();
//                    var result = pic.SaveImageInAlbum(Activity, _bmp);
//                    if (result) {
//                        Toast.MakeText(Activity, "±£´æ³É¹¦", ToastLength.Long);
//                    }
//                    else {
//                        Toast.MakeText(Activity, "±£´æÊ§°Ü", ToastLength.Long);
//                    }
//                }
//            };
			return dialog.Create();
		}

		private List<String> GetData()
		{

			List<String> data = new List<string>();
			data.Add("保存");

			return data;
		}
	}
}