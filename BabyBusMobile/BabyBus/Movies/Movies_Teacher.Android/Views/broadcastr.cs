 void ReachabilityChanged(object o, System.EventArgs e)
        {
            UpdateStatus();
            const string noNet = "(未连接)";
            if (remoteHostStatus == NetworkStatus.NotReachable
                && internetStatus == NetworkStatus.NotReachable
                && localWifiStatus == NetworkStatus.NotReachable)
            {
                foreach (var vc in ViewControllers)
                {
                    var nav = vc as UINavigationController;
                    if (nav != null)
                    {
                        var label = nav.TopViewController.NavigationItem.TitleView as UILabel;
                        if (label != null && !label.Text.Contains(noNet))
                        {
                            label.Text = label.Text + noNet;
                            label.Frame = new CoreGraphics.CGRect(label.Frame.X, label.Frame.Y, label.Frame.Width * 2, label.Frame.Height);
        
                        }
                    }
                }
            }
            else
            {
                foreach (var vc in ViewControllers)
                {
                    var nav = vc as UINavigationController;
                    if (nav != null)
                    {
                        var label = nav.TopViewController.NavigationItem.TitleView as UILabel;
                        if (label != null)
                        {
                            var start = label.Text.IndexOf(noNet, new System.StringComparison());
                            if (start >= 0 && start < label.Text.Length)
                            {
                                label.Text = label.Text.Substring(0, start);
                                label.Frame = new CoreGraphics.CGRect(label.Frame.X, label.Frame.Y, label.Frame.Width / 2, label.Frame.Height);
                            }
                        }
                    }
                }
            }
        }