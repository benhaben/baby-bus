using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using BabyBus.Droid.Views;
using BabyBus.Logic.Shared;

namespace BabyBus.Droid
{
	[Activity(Theme = "@style/CustomTheme", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]		
	public class LearningMaterialsView : ViewBase<LearningMaterialsViewModel>
	{
		private const string TitleResourceKey = "Title";
		private const string LinkResourceKey = "Link";
		List<IDictionary<string, object>> List;

		private const int TRIAL_COUNT = 3;

		private readonly string[] titles = {
			"经典儿歌",
			"经典故事",
			"幼儿百科",
			"亲子乐园",
		};

		private readonly string[] links = {
			"http://g.m.beva.com/?from=wwface",
			"http://t.m.beva.com/gushi/?from=wwface",
			"http://m.beva.com/baike/?from=wwface",
			"http://m.beva.com/qinzi/?from=wwface",
		};

		public override void OnCreateCalled(object sender, Cirrious.CrossCore.Core.MvxValueEventArgs<Bundle> e)
		{
			base.OnCreateCalled(sender, e);

			SetCustomTitleWithBack(Resource.Layout.Page_Content_Index, Resource.String.home_title_learning);

			// Create Simple Adapter
			var listview = FindViewById<ListView>(Resource.Id.content_list);
			List = new List<IDictionary<string, object>>();
			var baseUrl = Constants.BaseHtmlUrl + "/AlbumSongs_Mobile.html";

			ViewModel.FirstLoadedEventHandler += (sender2, arg) => RunOnUiThread(() => {
				for (int i = 0; i < ViewModel.Albums.Count; i++) {
					var album = ViewModel.Albums[i];
					var dic = new JavaDictionary<string, object>();
					if (!ViewModel.PaymentStatus && i < TRIAL_COUNT) {
						dic.Add(TitleResourceKey, album.Name + "(试听)");
						dic.Add(LinkResourceKey, string.Format(baseUrl + "?Id={0}", album.Id));
					} else {
						dic.Add(TitleResourceKey, album.Name);
						dic.Add(LinkResourceKey, string.Format(baseUrl + "?Id={0}", album.Id));
					}
					List.Add(dic);
				}

				for (int i = 0; i < titles.Length; i++) {
					var dic = new JavaDictionary<string, object>();
					dic.Add(TitleResourceKey, titles[i]);
					dic.Add(LinkResourceKey, links[i]);
					List.Add(dic);
				}
				var adapter = new SimpleAdapter(this,
					              List,
					              Resource.Layout.Item_Content,
					              new[] { TitleResourceKey },
					              new[] { Resource.Id.item_content_title });
				listview.Adapter = adapter;

				listview.ItemClick += (sender1, args) => {
					if (!ViewModel.PaymentStatus && args.Position >= TRIAL_COUNT) {
						ViewModel.ShowPaymentView();
					} else {
						var link = List[args.Position][LinkResourceKey].ToString();
						var title = List[args.Position][TitleResourceKey].ToString();
						var intent = new Intent(this, typeof(ContentDetailView));
						intent.PutExtra("FileName", link);
						intent.PutExtra("Title", title);
						StartActivity(intent);
					}
				};
			});
		}
	}
}

