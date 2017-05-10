using System;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Droid.Fragging;
using Color = Android.Graphics.Color;
using BabyBus.Logic.Shared;

namespace BabyBus.Droid.Views
{
	public class FragmentViewBase<TViewModel>
		: MvxFragmentActivity,ICustomTitleBarWithBack
		where TViewModel : BaseViewModel
	{
		public bool IsOnCreate{ get; set; }

		public new TViewModel ViewModel {
			get { return (TViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

		protected FragmentViewBase()
			: base()
		{
			InitEvent();
			IsOnCreate = true;
		}

		void InitEvent()
		{
			CreateCalled += OnCreateCalled;
			AfterCreateCalled += (sender, e) => {
				if (ViewModel != null) {
					ViewModel.FirstLoad();
				}
			}; 
		}

		public virtual void OnCreateCalled(object sender, Cirrious.CrossCore.Core.MvxValueEventArgs<Android.OS.Bundle> e)
		{
			Window.SetBackgroundDrawable(new ColorDrawable(new Color(255, 255, 255)));
			//HUD Info
			ViewModel.InfoChanged += ViewModel_InfoChanged;
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
				btnBack.Click += (object sender, EventArgs e) => CloseView();
				var topTitle = FindViewById<TextView>(Resource.Id.title);
				topTitle.Text = Resources.GetString(titleId);
			}

			RequestedOrientation = ScreenOrientation.Nosensor;
		}

		/// <summary>
		/// Set Custom Title With BackButton
		/// </summary>
		/// <param name="layoutId">ContentView Layout</param>
		/// <param name="title">Title String</param>
		public void SetCustomTitleWithBack(int layoutId, string title)
		{
			bool requestWindowFeature = false;
			requestWindowFeature = RequestWindowFeature(WindowFeatures.CustomTitle); 

			// Create your application here
			SetContentView(layoutId);

			if (requestWindowFeature) {
				Window.SetFeatureInt(WindowFeatures.CustomTitle, Resource.Layout.Bar_Default_WithBack);
				var btnBack = FindViewById<LinearLayout>(Resource.Id.ll_back);
				btnBack.Click += (object sender, EventArgs e) => CloseView();
				var topTitle = FindViewById<TextView>(Resource.Id.title);
				topTitle.Text = title;
			} 
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

		protected override void OnDestroy()
		{

			base.OnDestroy();
		}

		private void ViewModel_InfoChanged(object sender, EventArgs e)
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

