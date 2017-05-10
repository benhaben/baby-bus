using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;

namespace BabyBus.Droid.Views
{
	[Activity(Label = "ActivityBase")]
	public class ActivityBase : Activity
	{

		/// <summary>
		/// Set Custom Title With BackButton
		/// </summary>
		/// <param name="layoutId">ContentView Layout</param>
		/// <param name="title">Title String</param>
		protected virtual void SetCustomTitleWithBack(int layoutId, string title)
		{
			bool requestWindowFeature = false;
			requestWindowFeature = RequestWindowFeature(WindowFeatures.CustomTitle); 

			// Create your application here
			SetContentView(layoutId);

			if (requestWindowFeature) {
				Window.SetFeatureInt(WindowFeatures.CustomTitle, Resource.Layout.Bar_Default_WithBack);
				var btnBack = FindViewById<LinearLayout>(Resource.Id.ll_back);
				btnBack.Click += (object sender, EventArgs e) => {
					var newEvent = new KeyEvent(KeyEventActions.Down, Keycode.Back);
					OnKeyDown(Keycode.Back, newEvent);
				};
				var topTitle = FindViewById<TextView>(Resource.Id.title);
				topTitle.Text = title;
			} 
		}

		/// <summary>
		/// Set Custom Title With BackButton
		/// </summary>
		/// <param name="layoutId">ContentView Layout</param>
		/// <param name="titleId">Title String ResourceId</param>
		protected virtual void SetCustomTitleWithBack(int layoutId, int titleId)
		{
			bool requestWindowFeature = false;
			requestWindowFeature = RequestWindowFeature(WindowFeatures.CustomTitle); 

			// Create your application here
			SetContentView(layoutId);

			if (requestWindowFeature) {
				Window.SetFeatureInt(WindowFeatures.CustomTitle, Resource.Layout.Bar_Default_WithBack);
				var btnBack = FindViewById<LinearLayout>(Resource.Id.ll_back);
				btnBack.Click += (object sender, EventArgs e) => {
					var newEvent = new KeyEvent(KeyEventActions.Down, Keycode.Back);
					OnKeyDown(Keycode.Back, newEvent);
				};
				var topTitle = FindViewById<TextView>(Resource.Id.title);
				topTitle.Text = Resources.GetString(titleId);
			} 
		}

		protected virtual void SetShareTitleWithBack(int layoutId, int titleId)
		{
			bool requestWindowFeature = false;
			requestWindowFeature = RequestWindowFeature(WindowFeatures.CustomTitle); 

			// Create your application here
			SetContentView(layoutId);

			if (requestWindowFeature) {
				Window.SetFeatureInt(WindowFeatures.CustomTitle, Resource.Layout.Bar_Share_WithBack);
				var btnBack = FindViewById<LinearLayout>(Resource.Id.ll_back);
				btnBack.Click += (object sender, EventArgs e) => Finish();
				var btnShare = FindViewById<ImageView>(Resource.Id.icon_share);
				btnShare.Click += (sender, e) => ShareMessage();
				var topTitle = FindViewById<TextView>(Resource.Id.title);
				topTitle.Text = Resources.GetString(titleId);
			} 
		}

		protected virtual void SetShareTitleWithBack(int layoutId, string title)
		{
			bool requestWindowFeature = false;
			requestWindowFeature = RequestWindowFeature(WindowFeatures.CustomTitle); 

			// Create your application here
			SetContentView(layoutId);

			if (requestWindowFeature) {
				Window.SetFeatureInt(WindowFeatures.CustomTitle, Resource.Layout.Bar_Share_WithBack);
				var btnBack = FindViewById<LinearLayout>(Resource.Id.ll_back);
				btnBack.Click += (object sender, EventArgs e) => Finish();
				var btnShare = FindViewById<ImageView>(Resource.Id.icon_share);
				btnShare.Click += (sender, e) => ShareMessage();
				var topTitle = FindViewById<TextView>(Resource.Id.title);
				topTitle.Text = title;
			} 
		}

		public virtual void ShareMessage()
		{
		}

		public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
		{
			if (keyCode == Keycode.Back) {
				Finish();
				return false;
			}

			return base.OnKeyDown(keyCode, e);
		}
	}
}