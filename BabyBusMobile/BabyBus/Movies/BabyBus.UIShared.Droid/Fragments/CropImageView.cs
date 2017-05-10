//using System;
//using Android.Widget;
//using Android.Graphics;
//
//namespace BabyBus.Droid.Fragments
//{
//	public class CropImageView : ImageView
//	{
//		public bool IsCenterRoundCrop{ get; set; }
//
//		public RoundCropType RoundCropType{ get; set; }
//
//		public PaintFlagsDrawFilter mPaintFlagsDrawFilter;
//// 毛边过滤
//
//		protected override void OnDraw(Canvas canvas)
//		{
//			float h = GetMeasuredHeight() - 3.0f;
//			float w = getMeasuredWidth() - 3.0f;
//			if (path == null) {
//				path = new Path();
//				path.addCircle(
//					w / 2.0f
//					, h / 2.0f
//					, (float)Math.min(w / 2.0f, (h / 2.0))
//					, Path.Direction.CCW);
//				path.close();
//			}
//			canvas.DrawCircle(w / 2.0f, h / 2.0f, Math.min(w / 2.0f, h / 2.0f) + 1.5f, paint);
//			bool saveCount = canvas.GetClipBounds();
//			canvas.Save();
//			canvas.SetDrawFilter(mPaintFlagsDrawFilter);
//			canvas.ClipPath(path, Region.Op.REPLACE);
//			canvas.DrawColor(Color.White);
//			base.OnDraw(canvas);
//			canvas.RestoreToCount(saveCount);
//		}
//
//		public override void SetImageBitmap(Android.Graphics.Bitmap bm)
//		{
//			Bitmap output = Bitmap.CreateBitmap(bm.Width, bm.Height, Bitmap.Config.Argb8888);
//			Canvas canvas = new Canvas(output); 
//			Paint paint = new Paint();
//			//圆形罩罩的起始和结束坐标，以图片的左上角为0,0
//			Rect rect = new Rect(0, 0, bm.Width, bm.Height);
//			RectF rectF = new RectF(rect);
//			float roundPx = pixels;
//
//			paint.AntiAlias = true;
//			canvas.DrawARGB(0, 0, 0, 0);
//
//			paint.Color = Color.Red;
//			canvas.DrawRoundRect(rectF, bm.Width / roundPx, bm.Height / roundPx, paint);
//			paint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.SrcIn));
//			canvas.DrawBitmap(bm, rect, rect, paint);
//
//			//return output;
//
//		}
//			
//
//	}
//
//	public enum RoundCropType
//	{
//		Round = 2f,
//		RoundRect = 2f,
//
//	}
//
//}
//
