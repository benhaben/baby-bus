using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using BabyBus.Logic.Shared;

namespace BabyBus.Droid.Views.Setting
{
	[Activity(Label = "ÏêÏ¸×ÊÁÏ")]
	public class ChildInfoView : ViewBase<ChildInfoViewModel>
	{
		private Button approve;
		private Button refuse;

		protected override void OnCreate(Bundle bundle) {
			base.OnCreate(bundle);

			// Create your application here
			SetContentView(Resource.Layout.Page_Setting_ChildInfo);
			approve = FindViewById<Button>(Resource.Id.checkout_approve);
			refuse = FindViewById<Button>(Resource.Id.checkout_refuse);
			if (ViewModel.CheckoutAuditType == AuditType.Pending) {
				approve.Visibility = ViewStates.Visible;
				refuse.Visibility = ViewStates.Visible;
			} else {
				approve.Visibility = ViewStates.Gone;
				refuse.Visibility = ViewStates.Gone;
			}
		}
	}
}