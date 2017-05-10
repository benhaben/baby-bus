
using System.Collections.Generic;

using Android.App;
using Android.OS;
using Android.Widget;
using BabyBus.Droid.Views;
using BabyBus.Logic.Shared;
using Java.Lang;

namespace BabyBus.Droid
{
	[Activity(Theme = "@style/CustomTheme")]			
	public class SelectTestChildView : ViewBase<MIChildrenViewModel>
	{
		private Handler mHandler;
		private MISelectTestChildAdpter mAdapter;
		private  List<MITestMaster> _mi_testMaster = new List<MITestMaster>();

		public List<MITestMaster> MITestMaster {
			get { 
				return  _mi_testMaster ?? new List<MITestMaster>();
			}
			set { 
				_mi_testMaster = value;
			}
		}

		protected override void OnCreate(Bundle bundle) {
			base.OnCreate(bundle);
			MITestMaster = ViewModel.TestMasters;
			SetCustomTitleWithBack(Resource.Layout.Page_MI_SelectTestChild, "选择孩子");
			var gridview = FindViewById<GridView>(Resource.Id.modality_grid);

			mHandler = new Handler();
			mAdapter = new MISelectTestChildAdpter(this, MITestMaster);
			gridview.Adapter = mAdapter;

			ViewModel.FirstLoadedEventHandler += (sender, e) => mHandler.Post(new Runnable(() => {
				MITestMaster = ViewModel.TestMasters;
				mAdapter.list = MITestMaster;
				mAdapter.NotifyDataSetChanged();
			}));

			ViewModel.MasterMessageChange += (sender, e) => {
				MITestMaster = ViewModel.TestMasters;
				mAdapter.list = MITestMaster;
				mAdapter.NotifyDataSetChanged();
			};
		}
	}
}

