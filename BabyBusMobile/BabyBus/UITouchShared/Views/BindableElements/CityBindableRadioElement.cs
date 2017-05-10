using Cirrious.MvvmCross.Binding.BindingContext;
using BabyBus.Logic.Shared;


namespace BabyBus.iOS
{
    public class CityBindableRadioElement : MvxBindableRadioElement
    {
        public override void DoBind()
        {
            this.DelayBind(() =>
                {
                    var set = this.CreateBindingSet<MvxBindableRadioElement, CityModel>();
                    set.Bind().For(me => me.Caption).To(p => p.CityName);
                    set.Bind().For(me => me.Value).To(p => p.CityId);
                    //              set.Bind().For(me => me.DetailTestLableVisible).To(p => true);

                    set.Apply();
                });
        }
    }
}