using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Runtime;
using BabyBus.Droid.Adapters;
using Android.Support.V4.View;
using BabyBus.Droid.ViewPagerIndicator;
using Android.Widget;
using Android.Content;
using BabyBus.Logic.Shared;
using BabyBusSSApi.ServiceModel.Enumeration;

namespace BabyBus.Droid.Views.Main
{
	[Activity(Label = "TeacherHomeBaseViewModel")]
	public class TeacherHomeView : FragmentViewBase<TeacherHomeViewModel>
	{
		private const string ImageResourceKey = "ItemImage";
		private const string TextResourceKey = "TextImage";

		private readonly int[] images = {
			Resource.Drawable.Menu_Homework,
			Resource.Drawable.Menu_Notice,
			Resource.Drawable.Menu_Memory,
			Resource.Drawable.Menu_Attence,
			Resource.Drawable.Menu_Share,
			Resource.Drawable.menu_notice_3,
			Resource.Drawable.Menu_LearningMaterial,
			Resource.Drawable.Menu_telent_1,
		};

		public string[] texts = new string[8];

		protected override void OnCreate(Bundle bundle) {
			base.OnCreate(bundle);
			texts[0] = Resources.GetString(Resource.String.home_menu_homeworkmessage);
			texts[1] = Resources.GetString(Resource.String.home_menu_classnotice);
			texts[2] = Resources.GetString(Resource.String.home_menu_growmemory);
			texts[3] = Resources.GetString(Resource.String.home_menu_classattence);
			texts[4] = Resources.GetString(Resource.String.home_menu_parenteducation);
			texts[5] = Resources.GetString(Resource.String.home_menu_Personal_send);
			texts[6] = "幼教素材";
			texts[7] = "智能光谱";

			// Create your application here
			SetContentView(Resource.Layout.Page_Home_Teacher);

			var gridview = FindViewById<GridView>(Resource.Id.teacher_grid);
			//gridview.Adapter = new ButtonGroupAdapter(this);

			var list = new List<IDictionary<string, object>>();
			for (int i = 0; i < texts.Length; i++) {
				var dic = new JavaDictionary<string, object> {   
					{ ImageResourceKey,images[i] },
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
						ViewModel.NoticeType = NoticeType.ClassHomework;
						ViewModel.ShowSendNoticeCommand.Execute();
						break;
					case 1:
						ViewModel.NoticeType = NoticeType.ClassCommon;
						ViewModel.ShowSendNoticeCommand.Execute();
						break;
					case 2:
						ViewModel.NoticeType = NoticeType.GrowMemory;
						ViewModel.ShowSendNoticeCommand.Execute();
						break;
					case 3:
						ViewModel.ShowAttenceCommand.Execute();
						break;
					case 4:
						var parent = (MainView)Parent;
						var tabhost = parent.TabHost;
						tabhost.SetCurrentTabByTag("Question");
						break;
					case 5:
						ViewModel.ShowSendQuestionCommand.Execute();
						break;
					case 6:
						var intent_1 = new Intent(this, typeof(LearningMaterialView));
						StartActivity(intent_1);
						break;
					case 7:
						var intent_2 = new Intent(this, typeof(TeacherModalityView));
						StartActivity(intent_2);
						break;	
					default:
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