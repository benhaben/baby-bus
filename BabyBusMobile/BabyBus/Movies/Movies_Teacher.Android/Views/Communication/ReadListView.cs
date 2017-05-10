using Android.App;
using Android.OS;
using Android.Widget;
using BabyBus.Logic.Shared;
using BabyBus.Droid.Views;
using Java.Lang;

namespace BabyBus.Droid
{
	[Activity(Theme = "@style/CustomTheme")]		
	public class ReadListView : ViewBase<ReadListViewModel>
	{
		private readonly Handler mHandler = new Handler();

		protected override void OnCreate(Bundle bundle) {
			base.OnCreate(bundle);

			SetCustomTitleWithBack(Resource.Layout.Page_Setting_Children, Resource.String.mine_title_children);

			var list = FindViewById<ListView>(Resource.Id.children);
			var adapter = new ReadListAdapter(this, ViewModel.Readers);
			adapter.noticetype = ViewModel.Noticetype;
			list.Adapter = adapter;


			ViewModel.FirstLoadedEventHandler += (sender, args) => mHandler.PostDelayed(new Runnable(() => {
				adapter.List = ViewModel.Readers;
				adapter.NotifyDataSetChanged();
			}), 1000);
		}
	}
}

