using Cirrious.MvvmCross.Binding.BindingContext;
using UIKit;
using BabyBus.Logic.Shared;

namespace BabyBus.iOS
{
    public class KindergartenBindableRadioElement : MvxBindableRadioElement
    {
        public override void DoBind()
        {
            this.DelayBind(() =>
                {
                    var set = this.CreateBindingSet<MvxBindableRadioElement, KindergartenModel>();
                    set.Bind().For(me => me.Caption).To(p => p.Name);
                    set.Bind().For(me => me.Value).To(p => p.KindergartenId);
                    set.Apply();
                });
        }

        protected virtual void UpdateDetailDisplay(UITableViewCell cell)
        {
            var k = DataContext as KindergartenModel;
            cell.DetailTextLabel.Text = k.KindergartenName;
        }

    }
}