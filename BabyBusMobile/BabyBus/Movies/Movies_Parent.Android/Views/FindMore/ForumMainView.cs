
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
using BabyBus.Droid.Views;
using BabyBus.Logic.Shared;
using BabyBus.Droid.Control;
using Cirrious.MvvmCross.Droid.Views;

namespace BabyBus.Droid
{
	[Activity(Theme = "@style/CustomTheme", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]		
	public class ForumMainView : TabbedViewBase<ForumMainViewModel>
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Create your application here
		}

		protected override void OnViewModelSet()
		{
			var titlebar = "论坛";
			SetCustomTitleWithBack(Resource.Layout.Page_EC_ForumMain, titlebar);

			TabHost.TabSpec spec;

			for (int i = 0; i < ViewModel.CategoryList.Count; i++) {
				spec = TabHost.NewTabSpec(ViewModel.CategoryList[i].Name);
				spec.SetIndicator(ViewModel.CategoryList[i].Name);
				spec.SetContent(this.CreateIntentFor(ViewModel.ForumIndexViewModels[i]));
				TabHost.AddTab(spec);
			}
		}
	}
}

