
using System;
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
	public class TeacherModalityView : ViewBase<TeacherModalityViewModel>
	{
		private TeacherModalityAdapter mAdapter;
		private Handler mHandler;

		private List<MIModality> _modality;

		public List<MIModality> Modality {
			get {
				return _modality ?? new List<MIModality>();
			}
			set { 
				_modality = value;
			}
		}

		int[] ImageId = new int[] {
			Resource.Drawable.Modality_1,
			Resource.Drawable.Modality_2,
			Resource.Drawable.Modality_5,
			Resource.Drawable.Modality_6,
			Resource.Drawable.Modality_3,
			Resource.Drawable.Modality_4,
			Resource.Drawable.Modality_7,
			Resource.Drawable.Modality_8,
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

		protected override void OnCreate(Bundle bundle) {
			base.OnCreate(bundle);
			_modality = new List<MIModality>();
			//loadmodalityInfo ();
			SetCustomTitleWithBack(Resource.Layout.Page_MI_Modality, "智能光谱测评");
			var gridview = FindViewById<GridView>(Resource.Id.modality_grid);
			mAdapter = new TeacherModalityAdapter(this, Modality);
			mHandler = new Handler();
			gridview.Adapter = mAdapter;
			ViewModel.FirstLoadedEventHandler += (sender, e) => mHandler.Post(new Runnable(() => {
				_modality = ViewModel.TestModality;
				loadmodalityInfo();
				mAdapter.list = _modality;
				mAdapter.NotifyDataSetChanged();
			}));
			gridview.ItemClick += (sender, args) => {
				ViewModel.ShowDetailCommand(_modality[args.Position].ModalityId);
			};

			ViewModel.MasterMessageChange += (sender, e) => {
				_modality = ViewModel.TestModality;
				loadmodalityInfo();
				mAdapter.list = _modality;
				mAdapter.NotifyDataSetChanged();
			};
		}

		private void loadmodalityInfo() {
			if (Modality.Count >= 8) {
				for (var m = 0; m < 8; m++) {
					_modality[m].ModalityImageId = ImageId[m]; 
					_modality[m].ModalityName = ModalityName[m];
				}
			} else if (Modality.Count == 0) {
				for (var m = 0; m < 8; m++) {
					_modality.Add(new MIModality {
						ModalityImageId = ImageId[m],
						ModalityName = ModalityName[m],
					});
				}
			}
		}
	}
}

