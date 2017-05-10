using System;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using Cirrious.MvvmCross.Binding;
using Cirrious.CrossCore.Platform;
using System.Reflection;

namespace BabyBus.iOS
{
    public class MvxRadioRootElementEnhanceBinding<T> : MvxPropertyInfoTargetBinding<RadioRootElement<T>>
    {
        public MvxRadioRootElementEnhanceBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var rootElement = View;
            if (rootElement == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - rootElement is null in MvxRadioRootElementBinding");
            }
            else
            {
                rootElement.RadioSelectedChanged += RootElementOnRadioSelectedChanged;
            }
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.TwoWay; }
        }

        private void RootElementOnRadioSelectedChanged(object sender, EventArgs eventArgs)
        {
            FireValueChanged(View.EnhanceRadioSelected);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var rootElement = View;
                if (rootElement != null)
                {
                    rootElement.RadioSelectedChanged -= RootElementOnRadioSelectedChanged;
                }
            }
        }
    }
}

