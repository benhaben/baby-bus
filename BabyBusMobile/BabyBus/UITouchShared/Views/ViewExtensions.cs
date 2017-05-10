using System;
using UIKit;
using Cirrious.MvvmCross.Touch.Views;

namespace BabyBus.iOS
{
	public static class ViewExtensions
	{
		/// &lt;summary&gt;
		/// Find the first responder in the &lt;paramref name=&quot;view&quot;/&gt;'s subview hierarchy
		/// &lt;/summary&gt;
		/// &lt;param name=&quot;view&quot;&gt;ew
		/// A &lt;see cref=&quot;UIView&quot;/&gt;
		/// &lt;/param&gt;
		/// &lt;returns&gt;
		/// A &lt;see cref=&quot;UIView&quot;/&gt; that is the first responder or null if there is no first responder
		/// &lt;/returns&gt;
		public static UIView FindFirstResponder(this UIView view)
		{
			if (view.IsFirstResponder) {
				return view;
			}
			foreach (UIView subView in view.Subviews) {
				var firstResponder = subView.FindFirstResponder();
				if (firstResponder != null)
					return firstResponder;
			}
			return null;
		}

		/// &lt;summary&gt;
		/// Find the first Superview of the specified type (or descendant of)
		/// &lt;/summary&gt;
		/// &lt;param name=&quot;view&quot;&gt;
		/// A &lt;see cref=&quot;UIView&quot;/&gt;
		/// &lt;/param&gt;
		/// &lt;param name=&quot;stopAt&quot;&gt;
		/// A &lt;see cref=&quot;UIView&quot;/&gt; that indicates where to stop looking up the superview hierarchy
		/// &lt;/param&gt;
		/// &lt;param name=&quot;type&quot;&gt;
		/// A &lt;see cref=&quot;Type&quot;/&gt; to look for, this should be a UIView or descendant type
		/// &lt;/param&gt;
		/// &lt;returns&gt;
		/// A &lt;see cref=&quot;UIView&quot;/&gt; if it is found, otherwise null
		/// &lt;/returns&gt;
		public static UIView FindSuperviewOfType(
			this UIView view,
			UIView stopAt,
			Type type)
		{
			if (view.Superview != null) {
				if (type.IsAssignableFrom(view.Superview.GetType())) {
					return view.Superview;
				}

				if (view.Superview != stopAt)
					return view.Superview.FindSuperviewOfType(stopAt, type);
			}

			return null;
		}


	}
}

