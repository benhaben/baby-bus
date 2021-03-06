// MultilineElement.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using CoreGraphics;
using Foundation;
using UIKit;
using System;

namespace CrossUI.Touch.Dialog.Elements {
    public class MultilineElement : StringElement, IElementSizing {
        public MultilineElement(string caption = "")
            : base(caption) {
        }

        public MultilineElement(string caption, string value)
            : base(caption, value) {
        }

        public MultilineElement(string caption, Action tapped)
            : base(caption, tapped) {
        }

        protected override UITableViewCell GetCellImpl(UITableView tv) {
            var cell = base.GetCellImpl(tv);
            var tl = cell.TextLabel;
            tl.LineBreakMode = UILineBreakMode.WordWrap;
            tl.Lines = 0;

            return cell;
        }

        public virtual float GetHeight(UITableView tableView, NSIndexPath indexPath) {
            var size = new CGSize(280, float.MaxValue);
            using (var font = UIFont.FromName("Helvetica", 17f))
                return (float)(UIKit.UIStringDrawing.StringSize(Caption, font, size, UILineBreakMode.WordWrap).Height + 10);
        }
    }
}