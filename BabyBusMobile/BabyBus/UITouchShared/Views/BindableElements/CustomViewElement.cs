using Cirrious.MvvmCross.Binding.BindingContext;
using CrossUI.Touch.Dialog.Elements;

//using DialogExamples.Core.ViewModels;
using UIKit;

namespace BabyBus.iOS
{
    public class CustomViewElement<TViewTemplate>
		: UIViewElement
	  	, IBindableElement
		where TViewTemplate : UIView, new()
    {
        public IMvxBindingContext BindingContext { get; set; }

        public CustomViewElement(string caption, bool transparent)
            : base(caption, new TViewTemplate(), transparent)
        {
            this.CreateBindingContext();
        }

        public TViewTemplate ViewTemplate
        {
            get
            {
                return View as TViewTemplate;
            }
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