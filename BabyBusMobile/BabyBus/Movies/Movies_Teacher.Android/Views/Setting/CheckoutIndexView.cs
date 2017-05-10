using Android.App;
using Android.OS;
using Android.Widget;
using Java.Lang;
using Newtonsoft.Json;

namespace BabyBus.Droid.Views.Setting
{
	#if false
	[Activity(Label = "ÐÂÈëÔ°Ó×¶ù")]
	public class CheckoutIndexView : ViewBase<CheckoutIndexViewModel>
	{
		private readonly Handler mHandler = new Handler();
		private CheckoutListAdapter adapter;

		protected override void OnCreate(Bundle bundle) {
			base.OnCreate(bundle);

			// Create your application here
			SetContentView(Resource.Layout.Page_Setting_CheckoutIndex);

			var list = FindViewById<ListView>(Resource.Id.checkoutList);
			adapter = new CheckoutListAdapter(this, ViewModel.Checkouts);
			list.Adapter = adapter;


			list.ItemClick += (sender, args) => {
				CheckoutModel item = ViewModel.Checkouts[args.Position];
				string json = JsonConvert.SerializeObject(item);
				ViewModel.SelectedCheckoutJson = json;
				ViewModel.ShowDetailCommand.Execute();
			};

			ViewModel.FirstLoadedEventHandler += (sender, args) => mHandler.PostDelayed(new Runnable(() => {
				adapter.list = ViewModel.Checkouts;
				adapter.NotifyDataSetChanged();
				this.HideInfo();
			}), 1000);
		}

		protected override void OnResume() {
			base.OnResume();

			adapter.list = ViewModel.Checkouts;
			adapter.NotifyDataSetChanged();
		}
	}
	#endif
}