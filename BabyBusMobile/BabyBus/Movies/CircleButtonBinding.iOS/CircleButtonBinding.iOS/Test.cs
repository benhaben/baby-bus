
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace CircleButtonBinding.iOS
{
    public partial class Test : UIViewController
    {
        public Test () : base ("Test", null)
        {
        }

        public override void DidReceiveMemoryWarning ()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning ();
            
            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            
            // Perform any additional setup after loading the view, typically from a nib.
            View.BackgroundColor = UIColor.Blue;
            var button1 = new DKCircleButton ();
            button1.Frame = new RectangleF (0, 0, 90, 90);
//            button1.Center = new PointF (160, 200);
            button1.TitleLabel.Font = UIFont.SystemFontOfSize (22);
            button1.SetTitle ("KAISHI", UIControlState.Normal);
            button1.SetTitleColor (UIColor.Black, UIControlState.Normal);
//            button1.BorderColor = UIColor.Blue;
//            button1.TouchUpInside
            View.AddSubview (button1);
//            button1 = [[DKCircleButton alloc] initWithFrame:CGRectMake(0, 0, 90, 90)];
//
//            button1.center = CGPointMake(160, 200);
//            button1.titleLabel.font = [UIFont systemFontOfSize:22];
//
//            [button1 setTitleColor:[UIColor colorWithWhite:1 alpha:1.0] forState:UIControlStateNormal];
//            [button1 setTitleColor:[UIColor colorWithWhite:1 alpha:1.0] forState:UIControlStateSelected];
//            [button1 setTitleColor:[UIColor colorWithWhite:1 alpha:1.0] forState:UIControlStateHighlighted];
//
//            [button1 setTitle:NSLocalizedString(@"Start", nil) forState:UIControlStateNormal];
//            [button1 setTitle:NSLocalizedString(@"Start", nil) forState:UIControlStateSelected];
//            [button1 setTitle:NSLocalizedString(@"Start", nil) forState:UIControlStateHighlighted];
//
//            [button1 addTarget:self action:@selector(tapOnButton) forControlEvents:UIControlEventTouchUpInside];
//
//            [viewController.view addSubview:button1];
        }
    }
}

