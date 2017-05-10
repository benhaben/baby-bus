// StringElement.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Foundation;
using UIKit;

namespace CrossUI.Touch.Dialog.Elements
{
	public class StringElement : ValueElement<string>
	{
		private static readonly NSString Skey = new NSString ("StringElement");
		private static readonly NSString SkeyValue = new NSString ("StringElementValue");

		public StringElement (string caption = "") : base (caption)
		{
		}

		public StringElement (string caption, string value) : base (caption, value)
		{
		}

		public StringElement (string caption, Action tapped) : base (caption, tapped)
		{
		}

		protected override UITableViewCell GetCellImpl (UITableView tv)
		{
			var cell = tv.DequeueReusableCell (Value == null ? Skey : SkeyValue);
			if (cell == null) {
				cell = new UITableViewCell (Value == null ? UITableViewCellStyle.Default : UITableViewCellStyle.Value1,
					Skey);
				cell.SelectionStyle = IsSelectable
                                          ? UITableViewCellSelectionStyle.Blue
                                          : UITableViewCellSelectionStyle.None;
			}
			cell.Accessory = UITableViewCellAccessory.None;

			//Note : set transparent of cell
//			cell.BackgroundColor = UIColor.Clear;
//			cell.Opaque = true;
			return cell;
		}

		protected override void UpdateDetailDisplay (UITableViewCell cell)
		{
			if (cell == null)
				return;

			// The check is needed because the cell might have been recycled.
			if (cell.DetailTextLabel != null) {
				cell.DetailTextLabel.Text = Value ?? string.Empty;
				if (DetailTestLableVisible) {
					cell.DetailTextLabel.SetNeedsDisplay ();
				} else {
					cell.DetailTextLabel.Hidden = true;
				}
			}
		}

		private bool _detailTestLableVisible = false;

		/// <summary>
		///  Whether or not to display this element
		/// </summary>
		public bool DetailTestLableVisible {
			get { return _detailTestLableVisible; }
			set {
				if (_detailTestLableVisible == value)
					return;
				_detailTestLableVisible = value;
			}
		}

		public override bool Matches (string text)
		{
			return (Value != null ? Value.IndexOf (text, StringComparison.CurrentCultureIgnoreCase) != -1 : false) ||
			base.Matches (text);
		}
	}
}