using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;

namespace BabyBus.Droid.Views.Content
{
	[Activity(Theme = "@style/CustomTheme", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
	public class PhysicalTestView : ActivityBase
	{
		private const string TitleResourceKey = "Title";
		private const string LinkResourceKey = "Link";
		private readonly string[] titles = {
            "体质健康",
			"体质能检测",
            "骨密度检测",
        };

        private readonly string[] links =
        {
            "physical_test.htm",
            "fitness_test.htm",
            "bone_density_test.htm",
        };

        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

			SetCustomTitleWithBack (Resource.Layout.Page_Content_Index, Resource.String.home_title_phsical);

            // Create Simple Adapter
            var listview = FindViewById<ListView>(Resource.Id.content_list);
            var list = new List<IDictionary<string, object>>();
            for (int i = 0; i < titles.Length; i++) {
                var dic = new JavaDictionary<string, object>();
                dic.Add(TitleResourceKey, titles[i]);
                dic.Add(LinkResourceKey, links[i]);
                list.Add(dic);
            }
            var adapter = new SimpleAdapter(this,
                list,
                Resource.Layout.Item_Content,
                new[] { TitleResourceKey },
                new[] { Resource.Id.item_content_title });
            listview.Adapter = adapter;

            listview.ItemClick += (sender, args) => {
                var link = links[args.Position];
                var title = titles[args.Position];
                var intent = new Intent(this, typeof(ContentDetailView));
                intent.PutExtra("FileName", link);
                intent.PutExtra("Title", title);
                StartActivity(intent);
            };
        }
    }
}