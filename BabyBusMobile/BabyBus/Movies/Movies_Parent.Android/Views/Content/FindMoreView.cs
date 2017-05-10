using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace BabyBus.Droid.Views.Content
{
    [Activity(Label = "发现更多")]
    public class FindMoreView : ActivityBase
    {
        private const string TitleResourceKey = "Title";
        private const string LinkResourceKey = "Link";
        private readonly string[] titles =
        {
            "天赋基因检测",
            "国民体质监测中心",
            "陕西睿莱智能科技有限公司",
        };

        private readonly string[] links =
        {
            "FindMore/Talent gene detection.htm",
            "FindMore/ChinaPhysicalFitnessSurveillanceCenter.htm",
            "FindMore/RIT.htm",
        };
        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.Page_Content_Index);

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
                var intent = new Intent(this, typeof(ContentDetailView));
                intent.PutExtra("FileName", link);
                intent.PutExtra("Title", "宝宝测评");
                StartActivity(intent);
            };
        }
    }
}