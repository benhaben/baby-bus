using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Android.Support.V4.View;
using BabyBus.Droid.Adapters;
using BabyBus.Droid.ViewPagerIndicator;
using BabyBus.Logic.Shared;
using BabyBusSSApi.ServiceModel.Enumeration;

namespace BabyBus.Droid.Views.Main
{
	[Activity(Label = "MasterHomeViewModel")]
	public class MasterHomeView : FragmentViewBase<MasterHomeViewModel>
	{
		private const string ImageResourceKey = "ItemImage";
		private const string TextResourceKey = "TextImage";

		private readonly int[] images = {
			Resource.Drawable.Menu_Notice,
			Resource.Drawable.Menu_Staff,
			Resource.Drawable.Menu_Recipe,
			Resource.Drawable.Menu_Attence,
			Resource.Drawable.Menu_Mail,
		};

		public string[] texts = new string[5];

		protected override void OnCreate(Bundle bundle) {
			base.OnCreate(bundle);
			texts[0] = Resources.GetString(Resource.String.home_menu_kgnotice);
			texts[1] = Resources.GetString(Resource.String.home_menu_stuffnotice);
			texts[2] = "发送食谱";
			texts[3] = Resources.GetString(Resource.String.home_menu_kgattence);
			texts[4] = "园长信箱";

			// Create your application here
			SetContentView(Resource.Layout.Page_Home_Master);

			var gridview = FindViewById<GridView>(Resource.Id.teacher_grid);
//            gridview.Adapter = new ButtonGroupAdapter(this);

			var list = new List<IDictionary<string, object>>();
			for (int i = 0; i < images.Length; i++) {
				var dic = new JavaDictionary<string, object> {    //±ØÐëÊÇJavaDictionary¡£Dictionary»á³ö´í
					{ ImageResourceKey, images[i] },
					{ TextResourceKey, texts[i] }
				}; 
				list.Add(dic);
			}

			var saGroupButton = new SimpleAdapter(this,
				                    list,
				                    Resource.Layout.Item_ButtonGroup,
				                    new[] { ImageResourceKey, TextResourceKey },
				                    new[] {
					Resource.Id.MainActivityImage,
					Resource.Id.MainActivityText
				});

			gridview.Adapter = saGroupButton;

			gridview.ItemClick += (sender, args) => {
				switch (args.Position) {
					case 0:   
						ViewModel.NoticeType = NoticeType.KindergartenAll;
						ViewModel.ShowSendNoticeCommand.Execute();
						break;
					case 1:
						ViewModel.NoticeType = NoticeType.KindergartenStaff;
						ViewModel.ShowSendNoticeCommand.Execute();
						break;
					case 2:
						ViewModel.NoticeType = NoticeType.KindergartenRecipe;
						ViewModel.ShowSendNoticeCommand.Execute();
						break;
                        
					case 3:
						ViewModel.ShowAttenceCommand.Execute();
						break;
					case 4:
						((MainView)Parent).TabHost.SetCurrentTabByTag("Question");
						break;
				}
			};

			//View Pager Indictor
			var slideList = new List<int> {
				Resource.Drawable.bar_slide_1,
				Resource.Drawable.bar_slide_2,
				Resource.Drawable.bar_slide_3,
			};
			var pager = FindViewById<ViewPager>(Resource.Id.pager);
			var imageSlideAdapter = new ImageSlideAdapter(SupportFragmentManager){ List = slideList };
			var slideAdapter = imageSlideAdapter;
			pager.Adapter = slideAdapter;
			var indicator = FindViewById<CirclePageIndicator>(Resource.Id.indicator);
			indicator.SetViewPager(pager);
		}
	}
}