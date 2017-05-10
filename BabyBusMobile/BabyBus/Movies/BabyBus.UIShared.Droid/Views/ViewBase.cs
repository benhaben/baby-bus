using System;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Droid.Views;
using Color = Android.Graphics.Color;
using BabyBus.Logic.Shared;

namespace BabyBus.Droid.Views
{
	public class ViewBase<TViewModel>
		: MvxActivity, ICustomTitleBarWithBack
		where TViewModel : BaseViewModel
	{
		public bool IsOnCreate{ get; set; }

		public new TViewModel ViewModel {
			get { return (TViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

		protected ViewBase()
			: base()
		{
			InitEvent();
			IsOnCreate = true;
		}

		/// <summary>
		/// Set Custom Title With BackButton
		/// </summary>
		/// <param name="layoutId">ContentView Layout</param>
		/// <param name="titleId">Title String ResourceId</param>
		public void SetCustomTitleWithBack(int layoutId, int titleId)
		{
			bool requestWindowFeature = false;
			requestWindowFeature = RequestWindowFeature(WindowFeatures.CustomTitle); 

			// Create your application here
			SetContentView(layoutId);

			if (requestWindowFeature) {
				Window.SetFeatureInt(WindowFeatures.CustomTitle, Resource.Layout.Bar_Default_WithBack);
				var btnBack = FindViewById<LinearLayout>(Resource.Id.ll_back);
				btnBack.Click += (object sender, EventArgs e) => {
					CloseView();
				};
				var topTitle = FindViewById<TextView>(Resource.Id.title);
				topTitle.Text = Resources.GetString(titleId);
			} 

			RequestedOrientation = ScreenOrientation.Nosensor;
		}

		public void SetCalendarTitleWithBack(int layoutId, int titleId)
		{
			bool requestWindowFeature = false;
			requestWindowFeature = RequestWindowFeature(WindowFeatures.CustomTitle); 

			// Create your application here
			SetContentView(layoutId);

			if (requestWindowFeature) {
				Window.SetFeatureInt(WindowFeatures.CustomTitle, Resource.Layout.Bar_Calendar_WithBack);
				var btnBack = FindViewById<LinearLayout>(Resource.Id.ll_back);
				btnBack.Click += (object sender, EventArgs e) => CloseView();
				var topTitle = FindViewById<TextView>(Resource.Id.title);
				topTitle.Text = Resources.GetString(titleId);
			} 
		}

		public virtual void SetShareTitleWithBack(int layoutId, int titleId)
		{
			bool requestWindowFeature = false;
			requestWindowFeature = RequestWindowFeature(WindowFeatures.CustomTitle); 

			// Create your application here
			SetContentView(layoutId);

			if (requestWindowFeature) {
				Window.SetFeatureInt(WindowFeatures.CustomTitle, Resource.Layout.Bar_Share_WithBack);
				var btnBack = FindViewById<LinearLayout>(Resource.Id.ll_back);
				btnBack.Click += (object sender, EventArgs e) => CloseView();
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

		/// <summary>
		/// Set Custom Title With BackButton
		/// </summary>
		/// <param name="layoutId">ContentView Layout</param>
		/// <param name="title">Title String</param>
		/// 
		public static void ClearImage()
		{

		}

		public virtual void ShareMessage()
		{
		}

		public void SetCustomTitleWithBack(int layoutId, string title)
		{
			bool requestWindowFeature = false;
			requestWindowFeature = RequestWindowFeature(WindowFeatures.CustomTitle); 

			// Create your application here
			SetContentView(layoutId);

			if (requestWindowFeature) {
				Window.SetFeatureInt(WindowFeatures.CustomTitle, Resource.Layout.Bar_Default_WithBack);
				var btnBack = FindViewById<LinearLayout>(Resource.Id.ll_back);
				btnBack.Click += (object sender, EventArgs e) => {
					CloseView();
				};
				var topTitle = FindViewById<TextView>(Resource.Id.title);
				topTitle.Text = title;
			} 
		}

		public virtual void OnCreateCalled(object sender, Cirrious.CrossCore.Core.MvxValueEventArgs<Android.OS.Bundle> e)
		{
			Window.SetBackgroundDrawable(new ColorDrawable(new Color(255, 255, 255)));
			//HUD Info
			ViewModel.InfoChanged += ViewModel_InfoChanged;
		}

		/// <summary>
		/// Use Event to Protect Program in Order: First-Init View, Second-Load Data
		/// </summary>
		void InitEvent()
		{
			CreateCalled += OnCreateCalled;
			AfterCreateCalled += (sender, e) => {
				if (ViewModel != null) {
					ViewModel.FirstLoad();
				}
			}; 
		}

		protected override void OnPause()
		{
			base.OnPause();            
			if (IsFinishing)
				this.HideInfo();
		}

		void ViewModel_InfoChanged(object sender, EventArgs e)
		{
			RunOnUiThread(this.ShowInfo);
		}

		public virtual void CloseView()
		{
			Finish();
		}

		public override void OnBackPressed()
		{
			if (Parent != null) {
				Parent.OnBackPressed();
			} else {
				Finish();
			}
		}
	}


}
