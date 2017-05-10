using Android.App;
using Android.OS;
using Android.Widget;
using BabyBus.Droid.Adapters;
using Java.Lang;
using Newtonsoft.Json;
using BabyBus.Logic.Shared;

namespace BabyBus.Droid.Views.Setting
{
	[Activity(Label = "本班幼儿列表")]
	public class ChildrenView : ViewBase<ChildrenViewModel>
	{
		private readonly Handler mHandler = new Handler();

		protected override void OnCreate(Bundle bundle) {
			base.OnCreate(bundle);

			// Create your application here
			SetContentView(Resource.Layout.Page_Setting_Children);

			var list = FindViewById<ListView>(Resource.Id.children);
			var adapter = new ChildListAdapter(this, ViewModel.Children);
			list.Adapter = adapter;

			list.ItemClick += (sender, args) => {
				var item = ViewModel.Children[args.Position];
				var json = JsonConvert.SerializeObject(item);
				ViewModel.SelectedCheckoutJson = json;
				ViewModel.ShowDetailCommand.Execute();
			};

			ViewModel.FirstLoadedEventHandler += (sender, args) => mHandler.PostDelayed(new Runnable(() => {
				adapter.list = ViewModel.Children;
				adapter.NotifyDataSetChanged();
				this.HideInfo();
			}), 1000);
		}
	}
}

