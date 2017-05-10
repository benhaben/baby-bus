using System;
using Cirrious.MvvmCross.Binding.BindingContext;
using CrossUI.Touch.Dialog.Elements;



namespace BabyBus.iOS
{
    public class MvxBindableRadioElement : RadioElement, IBindableElement
    {
        public IMvxBindingContext BindingContext { get; set; }

        public MvxBindableRadioElement()
        {
            this.CreateBindingContext();

        }

        public virtual void DoBind()
        {
					
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                BindingContext.ClearAllBindings();
            }

            base.Dispose(disposing);
        }

        public virtual object DataContext
        {
            get { return BindingContext.DataContext; }
            set { BindingContext.DataContext = value; }
        }
    }
}

