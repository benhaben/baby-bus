//using System;
//using Android.Widget;
//using Android.Runtime;
//using Com.Squareup.Picasso;
//using System.Runtime.Remoting.Contexts;
//using Android.OS;
//
//namespace BabyBus.Droid.Utils.Image
//{
//	public class SampleScrollListener : AbsListView.IOnScrollListener
//	{
//		public SampleScrollListener(Context Context)
//		{
//			this.context = Context;
//		}
//		public void Dispose(){
//		
//		}
//		public Handler  Handler{ get;}
//		private readonly Context context;
//		private string someTag;
//		private bool IsFling;
//
//		public	void OnScroll(AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount)
//		{
//			
//		}
//		public void SetSampleScrollListener(string someTag){
//			OnScrollStateChanged(AbsListView view, [GeneratedEnum] ScrollState scrollState);
//		}
//		public void OnScrollStateChanged(AbsListView view, [GeneratedEnum] ScrollState scrollState)
//		{
//			Picasso picasso = Picasso.With(context);
//			if (scrollState == ScrollState.Fling ||
//			    scrollState == ScrollState.TouchScroll) {
//				IsFling = true;
//				picasso.ResumeTag(someTag);
//			} else {
//				picasso.PauseTag(someTag);
//			}
//		}
//
//	}
//}
//
