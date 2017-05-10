using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using BabyBus.Droid.Adapters;
using BabyBus.Droid.Views;
using BabyBus.Logic.Shared;
using Java.Lang;

namespace BabyBus.Droid.Views.Communication
{
	[Activity(Theme = "@style/CustomTheme")]
	public class SelectChildrenView : ViewBase<SelectChildrenViewModel>
	{
		private readonly Handler handler;

		public SelectChildrenView()
		{
			handler = new Handler();
		}

		public override void OnCreateCalled(object sender, Cirrious.CrossCore.Core.MvxValueEventArgs<Bundle> e)
		{
			base.OnCreateCalled(sender, e);
			SetCustomTitleWithBack(Resource.Layout.Page_SelectParents, "选择孩子");

			var bt = FindViewById<Button>(Resource.Id.childgrid_bt_finish);
//			bt.Click += (sender, e) => {
//				Finish();
//				CloseView();
//
//			};

			var grid = FindViewById<GridView>(Resource.Id.childgrid);
			var adapter = new ChildGridAdapter(this, ViewModel.Children);
			grid.Adapter = adapter;

			grid.ItemClick += (sender1, args1) => {
				View view = args1.View;
				var isselect = view.FindViewById<ImageView>(Resource.Id.signed);
				ChildModel child = ViewModel.Children[args1.Position];
				child.IsSelect = !child.IsSelect;
				isselect.Visibility = child.IsSelect ? ViewStates.Visible : ViewStates.Gone;
				bt.Text = string.Format("完成({0}/{1})", ViewModel.Attence, ViewModel.Total);
			};

			ViewModel.FirstLoadedEventHandler += (sender1, args1) => handler.Post(new Runnable(() => {
				adapter.list = ViewModel.Children;
				grid.Adapter = adapter;
				adapter.NotifyDataSetChanged();
			}));
		}
	}
}

