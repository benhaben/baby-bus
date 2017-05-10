
using System;
using System.Collections.Generic;

using Android.App;
using Android.OS;
using Android.Widget;
using BabyBus.Droid.Views;
using BabyBus.Logic.Shared;
using Java.Lang;
using Android.Util;

namespace BabyBus.Droid
{
	[Activity(Theme = "@style/CustomTheme", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]			
	public class ParentModalityView :  ViewBase<ParentModalityViewModel>
	{
		private List<MITestMaster> _modality;
		private Handler mHandler;
		private MITestMasterAdapter mAdapter;

		public List<MITestMaster> Modality {
			get {
				if (_modality == null) {
					_modality = new List<MITestMaster>();
					for (int i = 0; i < 8; i++) {
						var item = new MITestMaster {
							ModalityId = i + 1,
							ModalityImageId = ImageId[i],
							ModalityName = ModalityName[i],
							TotalTest = ModalityTestNo[i],
						};
						_modality.Add(item);
					}
				}
				return _modality;
			}
			set {
				_modality = value;
			}
		}

		int[] ImageId = new int[] {
			Resource.Drawable.modality_1,
			Resource.Drawable.modality_2,
			Resource.Drawable.modality_5,
			Resource.Drawable.modality_6,
			Resource.Drawable.modality_3,
			Resource.Drawable.modality_4,
			Resource.Drawable.modality_7,
			Resource.Drawable.modality_8,
		};
		string[] ModalityName = new string[8] {
			"语言言语智力",
			"数学逻辑智力",
			"视觉空间智力",
			"身体动觉智力",
			"音乐节奏智力",
			"人际交往智力",
			"自知自省智力",
			"自然观察智力",
		};

		int[] ModalityTestNo = new int[] {
			11, 11, 10, 10, 10, 10, 10, 10	
		};

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			DisplayMetrics metric = new DisplayMetrics();
			SetCustomTitleWithBack(Resource.Layout.Page_multipleTest, "智能光谱测评");


			NetContectStatus.registerReceiver(this);
			var netStatu = FindViewById<LinearLayout>(Resource.Id.no_net_lable);
			NetContectStatus.NetStatus(this, netStatu);

			var metrics = Resources.DisplayMetrics;
			BabyBusContext.WidthInDp = ConvertPixelsToDp(metrics.WidthPixels);
			BabyBusContext.HeightInDp = ConvertPixelsToDp(metrics.HeightPixels);

			//loadmodalityInfo ();
			mAdapter = new MITestMasterAdapter(this, Modality);
			var modalityList = FindViewById<ListView>(Resource.Id.modality_Item);
			modalityList.Adapter = mAdapter;

			mHandler = new Handler();
			ViewModel.FirstLoadedEventHandler += (sender, list) => mHandler.Post(new Runnable(() => {
//				Modality = ViewModel.TestMasters;
				loadmodalityInfo(ViewModel.TestMasters);
				mAdapter.list = Modality;
				mAdapter.NotifyDataSetChanged();

			}));

			ViewModel.MasterMessageChange += (sender, e) => {
				//Modality = ViewModel.TestMasters;
				loadmodalityInfo(ViewModel.TestMasters);
				mAdapter.list = Modality;
				mAdapter.NotifyDataSetChanged();
			};
		}

		private int ConvertPixelsToDp(float pixelValue)
		{
			var dp = (int)((pixelValue) / Resources.DisplayMetrics.Density);
			return dp;
		}

		private void loadmodalityInfo(List<MITestMaster> newlist)
		{
			foreach (var item in Modality) {
				foreach (var newitem in newlist) {
					if (item.ModalityId == newitem.ModalityId) {
						item.TestMasterId = newitem.TestMasterId;
						item.IsFinished = newitem.IsFinished;
						item.CompletedTest = newitem.CompletedTest;
					}
				}
			}
		}
			
	}
}

