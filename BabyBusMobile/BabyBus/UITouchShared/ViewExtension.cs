using System;
using UIKit;
using Cirrious.CrossCore;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using Cirrious.MvvmCross.Touch.Views;
using WeixinPayBinding.iOS;
using BabyBus.Logic.Shared;
using Cirrious.MvvmCross.Dialog.Touch;

namespace UITouchShared
{
	public static class ViewExtension
	{
		public static UIImage ImageWithColor(UIColor color, CGSize size)
		{
			CGRect rect = new CGRect(0, 0, size.Width, size.Height);

			UIGraphics.BeginImageContext(rect.Size);

			var context = UIGraphics.GetCurrentContext();

			context.SetFillColor(color.CGColor);

			context.FillRect(rect);

			UIImage img = UIGraphics.GetImageFromCurrentImageContext();

			UIGraphics.EndImageContext();

			return img;
		}

		public static UIImage ImageWithColor(this  UIImage image, UIColor color, CGSize size)
		{
			CGRect rect = new CGRect(0, 0, size.Width, size.Height);

			UIGraphics.BeginImageContext(rect.Size);

			var context = UIGraphics.GetCurrentContext();

			context.SetFillColor(color.CGColor);

			context.FillRect(rect);

			UIImage img = UIGraphics.GetImageFromCurrentImageContext();

			UIGraphics.EndImageContext();

			return img;
		}

		public static void SetBadge(this UITabBarItem tabBarItem)
		{
			tabBarItem.BadgeValue = "新";
		}

		public static void ResetBadge(this UITabBarItem tabBarItem)
		{
			tabBarItem.BadgeValue = null;
		}

		public static void DisposeEx(this UIView view)
		{
			const bool enableLogging = false;
			try {
				if (view.IsDisposedOrNull())
					return;

				var viewDescription = string.Empty;

				viewDescription = view.Description;
				Mvx.Trace("Destroying " + viewDescription);

				var disposeView = true;
				var disconnectFromSuperView = true;
				var disposeSubviews = true;
				var removeGestureRecognizers = false; // WARNING: enable at your own risk, may causes crashes
				var removeConstraints = true;
				var removeLayerAnimations = true;
				var associatedViewsToDispose = new List<UIView>();
				var otherDisposables = new List<IDisposable>();

				if (view is UIActivityIndicatorView) {
					var aiv = (UIActivityIndicatorView)view;
					if (aiv.IsAnimating) {
						aiv.StopAnimating();
					}
				} else if (view is UITableView) {
					var tableView = (UITableView)view;

					if (tableView.DataSource != null) {
						otherDisposables.Add(tableView.DataSource);
					}
					if (tableView.BackgroundView != null) {
						associatedViewsToDispose.Add(tableView.BackgroundView);
					}

					tableView.Source = null;
					tableView.Delegate = null;
					tableView.DataSource = null;
					tableView.WeakDelegate = null;
					tableView.WeakDataSource = null;
					associatedViewsToDispose.AddRange(tableView.VisibleCells ?? new UITableViewCell[0]);
				} else if (view is UITableViewCell) {
					var tableViewCell = (UITableViewCell)view;
					disposeView = false;
					disconnectFromSuperView = false;
					if (tableViewCell.ImageView != null) {
						associatedViewsToDispose.Add(tableViewCell.ImageView);
					}
				} else if (view is UICollectionView) {
					var collectionView = (UICollectionView)view;
					disposeView = false; 
					if (collectionView.DataSource != null) {
						otherDisposables.Add(collectionView.DataSource);
					}
					if (!collectionView.BackgroundView.IsDisposedOrNull()) {
						associatedViewsToDispose.Add(collectionView.BackgroundView);
					}
					//associatedViewsToDispose.AddRange(collectionView.VisibleCells ?? new UICollectionViewCell[0]);
					collectionView.Source = null;
					collectionView.Delegate = null;
					collectionView.DataSource = null;
					collectionView.WeakDelegate = null;
					collectionView.WeakDataSource = null;
				} else if (view is UICollectionViewCell) {
					var collectionViewCell = (UICollectionViewCell)view;
					disposeView = false;
					disconnectFromSuperView = false;
					if (collectionViewCell.BackgroundView != null) {
						associatedViewsToDispose.Add(collectionViewCell.BackgroundView);
					}
				} else if (view is UIWebView) {
					var webView = (UIWebView)view;
					if (webView.IsLoading)
						webView.StopLoading();
					webView.LoadHtmlString(string.Empty, null); // clear display
					webView.Delegate = null;
					webView.WeakDelegate = null;
				} else if (view is UIImageView) {
					var imageView = (UIImageView)view;
					if (imageView.Image != null) {
						otherDisposables.Add(imageView.Image);
						imageView.Image = null;
					}
				}

				var gestures = view.GestureRecognizers;
				if (removeGestureRecognizers && gestures != null) {
					foreach (var gr in gestures) {
						view.RemoveGestureRecognizer(gr);
						gr.Dispose();
					}
				}

				if (removeLayerAnimations && view.Layer != null) {
					view.Layer.RemoveAllAnimations();
				}

				if (disconnectFromSuperView && view.Superview != null) {
					view.RemoveFromSuperview();
				}

				var constraints = view.Constraints;
				if (constraints != null && constraints.Any() && constraints.All(c => c.Handle != IntPtr.Zero)) {
					view.RemoveConstraints(constraints);
					foreach (var constraint in constraints) {
						constraint.Dispose();
					}
				}

				foreach (var otherDisposable in otherDisposables) {
					otherDisposable.Dispose();
				}

				foreach (var otherView in associatedViewsToDispose) {
					otherView.DisposeEx();
				}

				var subViews = view.Subviews;
				if (view.Subviews != null) {
					foreach (var subView in view.Subviews) {
						try {
							subView.RemoveFromSuperview();
							subView.DisposeEx();
						} catch (Exception error) {
							Mvx.Trace(error.Message);
						}
					}
					Mvx.Trace("Resuming " + view.Description);
				}

				if (view is ISpecialDisposable) {
					((ISpecialDisposable)view).SpecialDispose();
				} else if (disposeView) {
					if (view.Handle != IntPtr.Zero)
						view.Dispose();
				}

				Mvx.Trace("Destroyed {0}", viewDescription);

			} catch (Exception error) {
				Mvx.Trace(error.Message);
			}
		}

   

		public static void RemoveFromSuperviewAndDispose(this UIView view)
		{
			view.RemoveFromSuperview();
			view.DisposeEx();
		}

		public static bool IsDisposedOrNull(this UIView view)
		{
			if (view == null)
				return true;

			if (view.Handle == IntPtr.Zero)
				return true;
			;

			return false;
		}

		public interface ISpecialDisposable
		{
			void SpecialDispose();
		}

		/// <summary>
		/// Shows the shared action sheet.
		/// </summary>
		public static void ShowSharedActionSheet(this MvxDialogViewController vc, ShareModel share)
		{
			UIAlertController sharedSheet = 
				UIAlertController.Create("分享给您的朋友", "", UIAlertControllerStyle.ActionSheet);

			sharedSheet.AddAction(
				UIAlertAction.Create("分享到微信", UIAlertActionStyle.Default, (action) => Share(WXScene.Session, share)));
			sharedSheet.AddAction(
				UIAlertAction.Create("分享到朋友圈", UIAlertActionStyle.Default, (action) => Share(WXScene.Timeline, share)));

			sharedSheet.AddAction(
				UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));

			vc.PresentViewController(sharedSheet, true, null);
		}

		public static void ShowSharedActionSheet(this MvxViewController vc, ShareModel share)
		{
			UIAlertController sharedSheet = 
				UIAlertController.Create("分享给您的朋友", "", UIAlertControllerStyle.ActionSheet);

			sharedSheet.AddAction(
				UIAlertAction.Create("分享到微信", UIAlertActionStyle.Default, (action) => Share(WXScene.Session, share)));
			sharedSheet.AddAction(
				UIAlertAction.Create("分享到朋友圈", UIAlertActionStyle.Default, (action) => Share(WXScene.Timeline, share)));

			sharedSheet.AddAction(
				UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));

			vc.PresentViewController(sharedSheet, true, null);
		}

		static void Share(WXScene scene, ShareModel share)
		{
			var req = new SendMessageToWXReq();
			WXWebpageObject webpage = new WXWebpageObject();
			webpage.WebpageUrl = Constants.BaseApiUrl + string.Format("/sharehtml?ContentType={0}&Id={1}", share.ContentType, share.Id);
			WXMediaMessage msg = new WXMediaMessage();
			msg.MediaObject = webpage;
			msg.Title = share.Title;
			msg.Description = share.Description;
			msg.SetThumbImage(UIImage.FromBundle("icon-512.png"));

			req.BText = false;
			req.Message = msg;
			req.Scene = (int)scene;

			WXApi.SendReq(req);
		}
	}
}

