using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UIKit;
using System.Reflection;
using CoreAnimation;
using CoreGraphics;

namespace BabyBus.iOS
{

	//    ContentView.ConstrainLayout (() =>
	//
	//        border.Frame.Top == ContentView.Frame.Top &&
	//        border.Frame.Height == 0.5f &&
	//        border.Frame.Left == ContentView.Frame.Left &&
	//        border.Frame.Right == ContentView.Frame.Right &&
	//
	//        nameLabel.Frame.Left == ContentView.Frame.Left + hpad &&
	//        nameLabel.Frame.Right == ContentView.Frame.GetMidX () - 5.5f &&
	//        nameLabel.Frame.Top >= ContentView.Frame.Top + vpad &&
	//        nameLabel.Frame.Bottom <= ContentView.Frame.Bottom - vpad &&
	//        nameLabel.Frame.GetMidY () == ContentView.Frame.GetMidY () &&
	//
	//        gestureView.Frame.Left <= ContentView.Frame.GetMidX () - 22 &&
	//        gestureView.Frame.Right >= scalarLabel.Frame.Right + 22 &&
	//        gestureView.Frame.Width >= 88 &&
	//        gestureView.Frame.Top == ContentView.Frame.Top &&
	//        gestureView.Frame.Bottom == ContentView.Frame.Bottom &&
	//
	//        scalarLabel.Frame.Bottom == ContentView.Frame.Bottom - vpad &&
	//        scalarLabel.Frame.Left == nameLabel.Frame.Right + 11 &&
	//        scalarLabel.Frame.Right <= ContentView.Frame.Right - hpad &&
	//        scalarLabel.Frame.GetMidY () == ContentView.Frame.GetMidY () &&
	//
	//        unitsLabel.Frame.Left == scalarLabel.Frame.Right + 1 &&
	//        unitsLabel.Frame.GetBaseline () == scalarLabel.Frame.GetBaseline ()
	//
	//    );
	public static class EasyLayout
	{

		public static readonly float RequiredPriority = 1000;

		public static readonly float HighPriority = 750;

		public static readonly float LowPriority = 250;

		public static nfloat SettingChildIconWidthAndHeight = 30;

		public static nfloat BarHeight = 44f;
		public static nfloat ImageAndTitleHeight = 200f;
		public static nfloat ImageAndTitleWidth = 117f;
		public static nfloat LineHeight = 1f;
		public static nfloat GoldenRatio = 0.618f;


		public static nfloat SmallButtonWidthAndHeight = 30f;
		public static nfloat NormalTextFieldHeight = 28f;
		public static nfloat NormalTextViewHeight = 35f;
		public static nfloat SmallTextFieldHeight = 20f;
		public static nfloat TextFieldWidth = 150f;
		public static nfloat CircleButtonHeight = 68f;
		public static nfloat HeightOfContent = 120f;
		public static nfloat HeightOfSelectImageCollection = 228;

		public static nfloat MarginTopToFrameWithoutBar = 50f;
		public static nfloat MarginTopToFrame = 70f;

		public static nfloat MarginLarge = 20f;
		public static nfloat MarginNormal = 10f;
		public static nfloat MarginMedium = 5f;
		public static nfloat MarginSmall = 2f;
		public static nfloat SeparatorHeight = 5f;
		public static nfloat HeadPortraitImageHeight = 40f;

		public static nfloat PhoneImageWidth = 18f;

		public static int MaximumImagesCount {
			get {
				if (UIScreen.MainScreen.Bounds.Height > 480) {
					return 9;
				} else {
					return 9;
				}
			}
		}

		public static nfloat PhotoStackHeight {
			get {
				if (UIScreen.MainScreen.Bounds.Height > 480) {
					return 300f;
				} else {
					return 250f;
				}
			}
		}

		public static nfloat PhotoStackHeightFullScreen {
			get {
				if (UIScreen.MainScreen.Bounds.Height > 480) {
					return 450;
				} else {
					return 380;
				}
			}
		}

		public static nfloat ImageFullScreenWidth {
			get {
				if (UIScreen.MainScreen.Bounds.Height > 480) {
					return 440;
				} else {
					return 380;
				}
			}
		}

		public static nfloat ImageFullScreenHeight = 470f;

		public static nfloat Red = 0f;
		public static nfloat Green = 0f;
		public static nfloat Blue = 0f;
		public static nfloat Alpha = 1.0f;
		public static nfloat BorderWidth = 1.0f;
		public static nfloat CornerRadius = 3.0f;

		//main staticbutton
		public static nfloat HomePageNoticeTabBarHeightAndWidth = 25f;

		//picture height(123) + page contorl(15)138
		public static nfloat AdvertiseBarHeight = 138;
		//        public static nfloat AdvertiseBarHeightOfTeacher = 205;

		public static nfloat AdvertiseBarWidth = 320f;

		public static nfloat HomePageNoticeBarHeight = 80;
		public static nfloat HomePageButtonImageWidth = 60;
		public static nfloat HomePageButtonImageHeight = 35f;
		public static nfloat HomePageButtonLabelHeight = 53f;
		public static nfloat HomePageButtonBarMargin = 35f;

		//AttendanceMasterView
		public static nfloat MaxLabelWidthInCell = 100f;
		public static nfloat MaxNumberWidthInCell = 30f;

		public static CGSize MaxLabelSizeInRow {
			get {
				return new CGSize(177, 300);
			}
		}

		public static UIFont HomePageButtonLabelFont {
			get {
				return UIFont.SystemFontOfSize(12);
			}
		}

		public static UIFont BigFontBold {
			get {
				return UIFont.BoldSystemFontOfSize(25);
			}
		}

		public static UIFont TitleFontBold {
			get {
				return UIFont.BoldSystemFontOfSize(16);
			}
		}

		public static UIFont TitleFont {
			get { 
				//                加粗;
				//                [UILabel setFont:[UIFont fontWithName:@"Helvetica-Bold" size:20]];
				//                加粗并且倾斜
				//                [UILabel setFont:[UIFont fontWithName:@"Helvetica-BoldOblique" size:20]];
				return UIFont.SystemFontOfSize(16);
			}
		}

		public static UIFont ContentFont {
			get { 
				return UIFont.SystemFontOfSize(14);
			}
		}

		public static UIFont SubTitleFont {
			get { 
				return UIFont.SystemFontOfSize(15);
			}
		}

		public static UIFont SmallFont {
			get { 
				return UIFont.SystemFontOfSize(13);
			}
		}

		public static UIFont TinyFont {
			get { 
				return UIFont.SystemFontOfSize(10);
			}
		}


		public static UIColor TextColor {
			get { 
				return UIColor.Black;
			}
		}

		static public CAShapeLayer CreateLineLayer(UIView view, UIColor color) {
			CGPath path = new CGPath();
			path.MoveToPoint(0, view.Frame.Size.Height);
			path.AddLineToPoint(view.Frame.Size.Width, view.Frame.Size.Height);
			CAShapeLayer line = new CAShapeLayer();
			line.Path = path;
			line.LineWidth = EasyLayout.LineHeight;
			line.Frame = view.Frame;
			line.StrokeColor = color.CGColor;
			return line;
			//this work too
			//                    _userName.Frame = new RectangleF (0, 0, EasyLayout.TextFieldWidth, EasyLayout.Height);
			//                    var border = new  CALayer ();
			//                    var width = 2.0f;
			//                    border.BorderColor = UIColor.LightGray.CGColor;
			//                    border.Frame = new RectangleF (0, _userName.Frame.Size.Height - width, _userName.Frame.Size.Width, _userName.Frame.Size.Height);
			//                    border.BorderWidth = width;
			//                    _userName.Layer.AddSublayer (border);
			//                    _userName.Layer.MasksToBounds = true;
		}


		/// <summary>
		/// <para>Constrains the layout of subviews according to equations and
		/// inequalities specified in <paramref name="constraints"/>.  Issue
		/// multiple constraints per call using the &amp;&amp; operator.</para>
		/// <para>e.g. button.Frame.Left &gt;= text.Frame.Right + 22 &amp;&amp;
		/// button.Frame.Width == View.Frame.Width * 0.42f</para>
		/// </summary>
		/// <param name="view">The superview laying out the referenced subviews.</param>
		/// <param name="constraints">Constraint equations and inequalities.</param>
		public static NSLayoutConstraint[] ConstrainLayout(
			this UIView view,
			Expression<Func<bool>> constraints) {
			return ConstrainLayout(
				view,
				constraints,
				UILayoutPriority.Required);
		}

		/// <summary>
		/// <para>Constrains the layout of subviews according to equations and
		/// inequalities specified in <paramref name="constraints"/>.  Issue
		/// multiple constraints per call using the &amp;&amp; operator.</para>
		/// <para>e.g. button.Frame.Left &gt;= text.Frame.Right + 22 &amp;&amp;
		/// button.Frame.Width == View.Frame.Width * 0.42f</para>
		/// </summary>
		/// <param name="view">The superview laying out the referenced subviews.</param>
		/// <param name="constraints">Constraint equations and inequalities.</param>
		/// <param name = "priority">The priority of the constraints</param>
		public static NSLayoutConstraint[] ConstrainLayout(
			this UIView view,
			Expression<Func<bool>> constraints,
			UILayoutPriority priority) {
			var body = constraints.Body;

			var exprs = new List<BinaryExpression>();
			FindConstraints(body, exprs);

			var layoutConstraints = exprs.Select(e => CompileConstraint(
				                        e,
				                        view)).ToArray();

			if (layoutConstraints.Length > 0) {
				foreach (var c in layoutConstraints) {
					c.Priority = (float)priority;
				}
				view.AddConstraints(layoutConstraints);
			}

			return layoutConstraints;
		}

		static NSLayoutConstraint CompileConstraint(
			BinaryExpression expr,
			UIView constrainedView) {
			var rel = NSLayoutRelation.Equal;
			switch (expr.NodeType) {
				case ExpressionType.Equal:
					rel = NSLayoutRelation.Equal;
					break;
				case ExpressionType.LessThanOrEqual:
					rel = NSLayoutRelation.LessThanOrEqual;
					break;
				case ExpressionType.GreaterThanOrEqual:
					rel = NSLayoutRelation.GreaterThanOrEqual;
					break;
				default:
					throw new NotSupportedException("Not a valid relationship for a constrain.");
			}

			var left = GetViewAndAttribute(expr.Left);
			if (left.Item1 != constrainedView) {
				left.Item1.TranslatesAutoresizingMaskIntoConstraints = false;
			}

			var right = GetRight(expr.Right);

			//            [Export("constraintWithItem:attribute:relatedBy:toItem:attribute:multiplier:constant:"), CompilerGenerated]
			//            public static NSLayoutConstraint Create(NSObject view1, NSLayoutAttribute attribute1, NSLayoutRelation relation, NSObject view2, NSLayoutAttribute attribute2, nfloat multiplier, nfloat constant);

			return NSLayoutConstraint.Create(
				left.Item1, left.Item2,
				rel,
				right.Item1, right.Item2,
				right.Item3, right.Item4);
		}

		static Tuple<UIView, NSLayoutAttribute, nfloat, nfloat> GetRight(Expression expr) {
			var r = expr;

			UIView view = null;
			NSLayoutAttribute attr = NSLayoutAttribute.NoAttribute;
			var mul = 1.0;
			var add = 0.0;
			var pos = true;

			if (r.NodeType == ExpressionType.Add || r.NodeType == ExpressionType.Subtract) {
				var rb = (BinaryExpression)r;
				if (IsConstant(rb.Left)) {
					add = ConstantValue(rb.Left);
					if (r.NodeType == ExpressionType.Subtract) {
						pos = false;
					}
					r = rb.Right;
				} else if (IsConstant(rb.Right)) {
					add = ConstantValue(rb.Right);
					if (r.NodeType == ExpressionType.Subtract) {
						add = -add;
					}
					r = rb.Left;
				} else {
					//                    add = Convert.ToSingle (Eval (r));
					//                    var a = Eval (rb.Right);
					//                    rb.Right.ReduceAndCheck ();
					throw new NotSupportedException("Addition only supports constants: " + rb.Right.NodeType);
				}
			}

			if (r.NodeType == ExpressionType.Multiply) {
				var rb = (BinaryExpression)r;
				if (IsConstant(rb.Left)) {
					mul = ConstantValue(rb.Left);
					r = rb.Right;
				} else if (IsConstant(rb.Right)) {
					mul = ConstantValue(rb.Right);
					r = rb.Left;
				} else {
					throw new NotSupportedException("Multiplication only supports constants.");
				}
			}

			if (IsConstant(r)) {
				add = Convert.ToDouble(Eval(r));
			} else if (r.NodeType == ExpressionType.MemberAccess || r.NodeType == ExpressionType.Call) {
				var t = GetViewAndAttribute(r);
				view = t.Item1;
				attr = t.Item2;
			} else {
				//                var b = r.CanReduce;
				//                var bb = r.Reduce ();
				throw new NotSupportedException("Unsupported layout expression node type " + r.NodeType);
			}

			if (!pos)
				mul = -mul;

			return Tuple.Create(view, attr, (nfloat)mul, (nfloat)add);
		}

		static bool IsConstant(Expression expr) {
			if (expr.NodeType == ExpressionType.Constant)
				return true;

			if (expr.NodeType == ExpressionType.MemberAccess) {
				var mexpr = (MemberExpression)expr;
				var m = mexpr.Member;
				if (m.MemberType == MemberTypes.Field) {
					return true;
				}
				return false;
			}

			if (expr.NodeType == ExpressionType.Convert) {
				var cexpr = (UnaryExpression)expr;
				return IsConstant(cexpr.Operand);
			}

			return false;
		}

		static nfloat ConstantValue(Expression expr) {
			return Convert.ToSingle(Eval(expr));
		}

		static Tuple<UIView, NSLayoutAttribute> GetViewAndAttribute(Expression expr) {
			var attr = NSLayoutAttribute.NoAttribute;
			MemberExpression frameExpr = null;

			var fExpr = expr as MethodCallExpression;
			if (fExpr != null) {
				switch (fExpr.Method.Name) {
					case "GetMidX":
					case "GetCenterX":
						attr = NSLayoutAttribute.CenterX;
						break;
					case "GetMidY":
					case "GetCenterY":
						attr = NSLayoutAttribute.CenterY;
						break;
					case "GetBaseline":
						attr = NSLayoutAttribute.Baseline;
						break;
					default:
						throw new NotSupportedException("Method " + fExpr.Method.Name + " is not recognized.");
				}

				frameExpr = fExpr.Arguments.FirstOrDefault() as MemberExpression;
			}

			if (attr == NSLayoutAttribute.NoAttribute) {
				var memExpr = expr as MemberExpression;
				if (memExpr == null)
					throw new NotSupportedException("Left hand side of a relation must be a member expression, instead it is " + expr);

				switch (memExpr.Member.Name) {
					case "Width":
						attr = NSLayoutAttribute.Width;
						break;
					case "Height":
						attr = NSLayoutAttribute.Height;
						break;
					case "Left":
					case "X":
						attr = NSLayoutAttribute.Left;
						break;
					case "Top":
					case "Y":
						attr = NSLayoutAttribute.Top;
						break;
					case "Right":
						attr = NSLayoutAttribute.Right;
						break;
					case "Bottom":
						attr = NSLayoutAttribute.Bottom;
						break;
					default:
						throw new NotSupportedException("Property " + memExpr.Member.Name + " is not recognized.");
				}

				frameExpr = memExpr.Expression as MemberExpression;
			}

			if (frameExpr == null)
				throw new NotSupportedException("Constraints should use the Frame or Bounds property of views.");
			var viewExpr = frameExpr.Expression;

			var view = Eval(viewExpr) as UIView;
			if (view == null)
				throw new NotSupportedException("Constraints only apply to views.");

			return Tuple.Create(view, attr);
		}

		static object Eval(Expression expr) {
			if (expr.NodeType == ExpressionType.Constant) {
				return ((ConstantExpression)expr).Value;
			}

			if (expr.NodeType == ExpressionType.MemberAccess) {
				var mexpr = (MemberExpression)expr;
				var m = mexpr.Member;
				if (m.MemberType == MemberTypes.Field) {
					var f = (FieldInfo)m;
					object v;
					if (mexpr.Expression == null) {
						//static
						v = f.GetValue(mexpr);
					} else {
						v = f.GetValue(Eval(mexpr.Expression));
					}
					return v;
				}
			}

			if (expr.NodeType == ExpressionType.Convert) {
				var cexpr = (UnaryExpression)expr;
				var op = Eval(cexpr.Operand);
				return Convert.ChangeType(op, cexpr.Type);
			}

			return Expression.Lambda(expr).Compile().DynamicInvoke();
		}

		static void FindConstraints(
			Expression expr,
			List<BinaryExpression> constraintExprs) {
			var b = expr as BinaryExpression;
			if (b == null)
				return;

			if (b.NodeType == ExpressionType.AndAlso) {
				FindConstraints(b.Left, constraintExprs);
				FindConstraints(b.Right, constraintExprs);
			} else {
				constraintExprs.Add(b);
			}
		}

		/// <summary>
		/// The baseline of the view whose frame is viewFrame. Use only when defining constraints.
		/// </summary>
		public static nfloat GetBaseline(this CGRect viewFrame) {
			return 0;
		}

		/// <summary>
		/// The x coordinate of the center of the frame. Use only when defining constraints.
		/// </summary>
		public static nfloat GetCenterX(this CGRect viewFrame) {
			return viewFrame.X + viewFrame.Width / 2;
		}

		/// <summary>
		/// The y coordinate of the center of the frame. Use only when defining constraints.
		/// </summary>
		public static nfloat GetCenterY(this CGRect viewFrame) {
			return viewFrame.Y + viewFrame.Height / 2;
		}
	}
}