
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;
using Android.Graphics;

namespace BabyBus.Droid.ViewPagerIndicator
{
		
	public class CircleImageView : ImageView
	{
		Android.Graphics.Path path;
		public PaintFlagsDrawFilter mPaintFlagsDrawFilter;// 毛边过滤
		Paint paint;


		public CircleImageView(Context context, Attribute attrs, int defStyle) {
			base.Context = context;
			// TODO Auto-generated constructor stub
			Init();
		}
		public void Init(){
			mPaintFlagsDrawFilter = new PaintFlagsDrawFilter(0,
				Paint.ANTI_ALIAS_FLAG|Paint.FILTER_BITMAP_FLAG);
			paint = new Paint();
			paint.SetAntiAlias(true);
			paint.SetFilterBitmap(true);
			paint.SetColor(Color.White);

		}
		protected override void OnDraw (Canvas canvas)
		{
			
			float h = GetMeasuredHeight()- 3.0f;
			float w = GetMeasuredWidth()- 3.0f;
			if (path == null) {
				path = new Android.Graphics.Path();
				path.AddCircle(
					w/2.0f, h/2.0f, (float) Math.Min(w/2.0f, (h / 2.0)), Path.Direction.Ccw);
				path.Close();
			}
			canvas.DrawCircle(w/2.0f, h/2.0f,  Math.Min(w/2.0f, h / 2.0f) + 1.5f, paint);
			int saveCount = canvas.SaveCount;
			canvas.Save();
			canvas.DrawPaint(mPaintFlagsDrawFilter);
			canvas.ClipPath(path,Region.Op.Replace);
			canvas.DrawPaint(mPaintFlagsDrawFilter);
			canvas.DrawColor(Color.White);
			base.OnDraw (canvas);
			canvas.RestoreToCount(saveCount);
		}
	}
}

