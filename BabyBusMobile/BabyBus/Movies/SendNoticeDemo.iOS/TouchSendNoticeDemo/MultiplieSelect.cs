using System;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using MonoTouch.ObjCRuntime;
using System.Drawing;
using MonoTouch.Foundation;

namespace TouchSendNoticeDemo
{

    public partial class MultiplieSelect : DialogViewController
    {
        public MultiplieSelect ()
            : base (UITableViewStyle.Grouped,
                    null,
                    false)
        {
            int i = 0;
            this.RefreshRequested += delegate {
                // Wait 3 seconds, to simulate some network activity
                NSTimer.CreateScheduledTimer (1, delegate {
                    Root [0].Add (new StringElement ("Added " + (++i)));

                    // Notify the dialog view controller that we are done
                    // this will hide the progress info
                    this.ReloadComplete ();
                });
            };
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

            UIBarButtonItem backItem = new UIBarButtonItem ();
            backItem.Title = "回不";
            this.NavigationItem.BackBarButtonItem = backItem;
            this.NavigationItem.SetRightBarButtonItem (
                new UIBarButtonItem (UIBarButtonSystemItem.Save, (sender, args) => {
                   
                })
                , true);

            // ios7 layout
            if (RespondsToSelector (new Selector ("edgesForExtendedLayout")))
                EdgesForExtendedLayout = UIRectEdge.None;


            // Perform any additional setup after loading the view, typically from a nib.
            Root = (RootElement)GetRoot ();
            //          GlassButton

            //
            // After the DialogViewController is created, but before it is displayed
            // Assign to the RefreshRequested event.   The event handler typically
            // will queue a network download, or compute something in some thread
            // when the update is complete, you must call "ReloadComplete" to put
            // the DialogViewController in the regular mode
            //
           

//            var btn = new UIButton (new RectangleF (50, 50, 100, 100));
//            btn.SetTitle ("test", UIControlState.Normal);
//            btn.BackgroundColor = UIColor.Black;
//            this.View.InsertSubview (btn, 10);
//            this.TableView.InsertSubview (btn, 10);
        }

        RootElement GetRoot ()
        {
            return new RootElement ("Filter Manager") { 
                new Section ("Section 1") { 
                    new RootElement ("Type 1", new  RadioGroup (0)) { 
                        new Section () { 
                            new CheckboxElement ("choice 1"), 
                            new CheckboxElement ("choice 2"), 
                            new CheckboxElement ("choice 3"), 
                            new CheckboxElement ("choice 4"), 
                            new CheckboxElement ("choice 5"), 
                        }   
                    }, 
                    new RootElement ("Type 2", new RadioGroup (0)) { 
                        new Section () { 
                            new CheckboxElement ("choice 1"), 
                            new CheckboxElement ("choice 2"), 
                            new CheckboxElement ("choice 3"), 
                            new CheckboxElement ("choice 4"), 
                        } 
                    }, 

                    new Section ("Search") { 
                        new EntryElement ("Asset", "Enter something", String.Empty), 
                        new EntryElement ("Description", "Enter something", String.Empty) 
                    }, 
                } 
            };
        }
    }
}

