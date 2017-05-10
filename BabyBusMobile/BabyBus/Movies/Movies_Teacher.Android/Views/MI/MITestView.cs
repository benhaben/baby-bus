
using System;
using System.Collections.Generic;

using Android.App;
using Android.OS;
using Android.Widget;
using BabyBus.Droid.Views;
using BabyBus.Logic.Shared;
using Java.Lang;
using Cirrious.MvvmCross.ViewModels;

namespace BabyBus.Droid
{
	[Activity(Theme = "@style/CustomTheme", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]			
	public class MITestView : ViewBase<MITestViewModel>
	{
		private  List<MIAssessIndex> _mi_testAssessList = new List<MIAssessIndex>();
		private float dp;
		private Handler mhandler;
		private MITestDetailAdapter mAdapter;

		public List<MIAssessIndex> MI_TestAssessList {
			get { 
				return _mi_testAssessList ?? new List<MIAssessIndex>();
			}
			set { 
				_mi_testAssessList = value;
			}
		}

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
		
			dp = (float)Resources.DisplayMetrics.Density;
			_mi_testAssessList = ViewModel.AssessIndexList;
			mAdapter = new  MITestDetailAdapter(this, MI_TestAssessList, dp);
			mhandler = new Handler();

			SetCustomTitleWithBack(Resource.Layout.Page_MI_TestDetail, "详细");
			var Commit = FindViewById<TextView>(Resource.Id.commit);
			Commit.Click += (sender, e) => {
				Simpleer("提示", "您确定要要发送,需要检查一下吗？", ViewModel.SendQuestions);

			};

			NetContectStatus.registerReceiver(this);
			var netStatu = FindViewById<LinearLayout>(Resource.Id.no_net_lable);
			NetContectStatus.NetStatus(this, netStatu);

			var metrics = Resources.DisplayMetrics;
			BabyBusContext.WidthInDp = ConvertPixelsToDp(metrics.WidthPixels);
			BabyBusContext.HeightInDp = ConvertPixelsToDp(metrics.HeightPixels);

			var gridview = FindViewById<ListView>(Resource.Id.test_assess);
			gridview.Adapter = mAdapter;

			ViewModel.FirstLoadedEventHandler += (sender, e) => mhandler.Post(new Runnable(() => {
				MI_TestAssessList = ViewModel.AssessIndexList;
				mAdapter.list = MI_TestAssessList;
				mAdapter.NotifyDataSetChanged();
			}));
		}

		private int ConvertPixelsToDp(float pixelValue)
		{
			var dp = (int)((pixelValue) / Resources.DisplayMetrics.Density);
			return dp;
		}

		private void Simpleer(string title, string message, MvxCommand cmd)
		{
			AlertDialog.Builder buider = new AlertDialog.Builder(this);
			buider.SetTitle(title);
			buider.SetMessage(message);
			buider.SetPositiveButton(Resource.String.check, ((sender, e) => {
				
			}));
			buider.SetNegativeButton(Resource.String.send, ((sender, e) => cmd.Execute()));
			buider.Create();
			buider.Show();
		}
			
	}
}

