//	New implementations, refactoring and restyling - FactoryMind || http://factorymind.com 
//  Converted to MonoTouch on 1/22/09 - Eduardo Scoz || http://escoz.com
//  Originally reated by Devin Ross on 7/28/09  - tapku.com || http://github.com/devinross/tapkulibrary
//
/*
 
 Permission is hereby granted, free of charge, to any person
 obtaining a copy of this software and associated documentation
 files (the "Software"), to deal in the Software without
 restriction, including without limitation the rights to use,
 copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the
 Software is furnished to do so, subject to the following
 conditions:
 
 The above copyright notice and this permission notice shall be
 included in all copies or substantial portions of the Software.
 
 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 OTHER DEALINGS IN THE SOFTWARE.
 
 */

using System;
using UIKit;

namespace BabyBus.iOS
{
    sealed class CalendarDayView : UIView
    {
        string text;
        int fontSize = 15;

        public DateTime Date { get; set; }

        bool _active, _today, _selected, _marked, _available;

        public bool Available
        {
            get { return _available; }
            set
            {
                _available = value;
                SetNeedsDisplay();
            }
        }

        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                SetNeedsDisplay();
            }
        }

        public bool Active
        {
            get { return _active; }
            set
            {
                _active = value;
                SetNeedsDisplay();
            }
        }

        public bool Today
        {
            get { return _today; }
            set
            {
                _today = value;
                SetNeedsDisplay();
            }
        }

        public bool Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                SetNeedsDisplay();
            }
        }

        public bool Marked
        {
            get { return _marked; }
            set
            {
                _marked = value;
                SetNeedsDisplay();
            }
        }

        public UIColor SelectionColor { get; set; }

        public UIColor TodayMarkColor { get; set; }

        public UIColor TodayCircleColor { get; set; }

        public CalendarDayView()
        {
            this.BackgroundColor = UIColor.White;
            this.TodayCircleColor = UIColor.Red;
            this.SelectionColor = UIColor.Red;
            this.TodayMarkColor = UIColor.White;
        }

        public override void Draw(CoreGraphics.CGRect rect)
        {
            PerformDraw();
        }

        void DrawToday()
        {
            var context = UIGraphics.GetCurrentContext();
            var todaySize = (float)Math.Min(Bounds.Height, Bounds.Width);
            if (todaySize > 40)
                todaySize = 40;
            todaySize = Math.Min(fontSize * 2, todaySize);
            TodayCircleColor.SetColor();
            context.SetLineWidth(0);
            context.AddEllipseInRect(new CoreGraphics.CGRect((Bounds.Width / 2) - (todaySize / 2), (Bounds.Height / 2) - (todaySize / 2) + 2, todaySize, todaySize));
            context.FillPath();

            MvxTouchColor.White.SetColor();
            Text.DrawString(new CoreGraphics.CGRect(0, (Bounds.Height / 2) - (fontSize / 2), Bounds.Width, Bounds.Height),
                UIFont.SystemFontOfSize(fontSize), UILineBreakMode.WordWrap,
                UITextAlignment.Center);
        }

        void DrawMark()
        {
            var context = UIGraphics.GetCurrentContext();

            if (!Active || !Available)
            {
                UIColor.LightGray.SetColor();
                Text.DrawString(new CoreGraphics.CGRect(0, (Bounds.Height / 2) - (fontSize / 2), Bounds.Width, Bounds.Height),
                    UIFont.SystemFontOfSize(fontSize), UILineBreakMode.WordWrap,
                    UITextAlignment.Center);
            }
            else
            {
                if (Selected && !Today)
                {
                    SelectionColor.SetColor();
                }
                else if (Today)
                {
                    TodayMarkColor.SetColor();
                }
                else
                {
                    MvxTouchColor.LightRed.SetColor();
                }
            
//                var size = (float)Math.Min(Bounds.Height, Bounds.Width);
//                if (size > 28)
//                    size = 28;
//                size = Math.Min(fontSize * 2, size);

                context.SetLineWidth(0);
                context.AddEllipseInRect(new CoreGraphics.CGRect(Frame.Size.Width / 2 - 2, (Bounds.Height / 2) + (fontSize / 2) + 5, 4, 4));

//                context.AddEllipseInRect(new CoreGraphics.CGRect((Bounds.Width / 2) - (size / 2), (Bounds.Height / 2) - (size / 2) + 2, size, size));
                context.FillPath();

                MvxTouchColor.Black1.SetColor();

                Text.DrawString(new CoreGraphics.CGRect(0, (Bounds.Height / 2) - (fontSize / 2), Bounds.Width, Bounds.Height),
                    UIFont.SystemFontOfSize(fontSize), UILineBreakMode.WordWrap,
                    UITextAlignment.Center);
            }
        }

        private void PerformDraw()
        {
            UIColor color = MvxTouchColor.Black1;

            if (!Active || !Available)
            {
                color = UIColor.LightGray;
                if (Selected)
                    color = SelectionColor;
            }
            else if (Today && Selected)
            {
                color = UIColor.White;
            }
            else if (Today)
            {
                color = UIColor.White;
            }
            else if (Selected)
            {
                color = SelectionColor;
            }

            if (Today)
            {
                DrawToday();
            }

            //今天也要考虑有没有mark
            if (Marked)
            {
                DrawMark();
            }
            else
            {
                color.SetColor();

                Text.DrawString(new CoreGraphics.CGRect(0, (Bounds.Height / 2) - (fontSize / 2), Bounds.Width, Bounds.Height),
                    UIFont.SystemFontOfSize(fontSize), UILineBreakMode.WordWrap,
                    UITextAlignment.Center);
            }
        }
    }
}