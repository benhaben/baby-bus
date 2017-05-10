using Cirrious.MvvmCross.Binding.BindingContext;
using BabyBus.Logic.Shared;

namespace BabyBus.iOS
{
    public class GenderBindableRadioElement : MvxBindableRadioElement
    {
        public override void DoBind()
        {
            this.DelayBind(() =>
                {
                    var set = this.CreateBindingSet<MvxBindableRadioElement, GenderModel>();
                    set.Bind().For(me => me.Caption).To(p => p.Gender);
                    set.Bind().For(me => me.Value).To(p => p.Id);
                    set.Apply();
                });
        }
    }
}