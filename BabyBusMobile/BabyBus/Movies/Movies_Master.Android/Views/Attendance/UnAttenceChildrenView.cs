using System;

using Android.App;
using Android.OS;
using Android.Widget;
using Newtonsoft.Json;
using BabyBus.Logic.Shared;

namespace BabyBus.Droid.Views.Attendance
{
	#if false
	[Activity(Label = "缺席幼儿")]
	public class UnAttenceChildrenView : ViewBase<UnattenceChildrenViewModel>
	{
		protected override void OnCreate(Bundle bundle) {
			base.OnCreate(bundle);

			// Create your application here
			SetContentView(Resource.Layout.Page_Setting_Children);

			var list = FindViewById<ListView>(Resource.Id.children);
			list.ItemClick += (sender, args) => {
				var item = ViewModel.Children[args.Position];
				var json = JsonConvert.SerializeObject(item);
				ViewModel.SelectedCheckoutJson = json;
				ViewModel.ShowDetailCommand.Execute();
			};
		}
	}
	#endif
}