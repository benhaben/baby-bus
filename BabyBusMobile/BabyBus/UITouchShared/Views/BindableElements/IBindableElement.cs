using Cirrious.MvvmCross.Binding.BindingContext;

namespace BabyBus.iOS
{
	public interface IBindableElement
        : IMvxBindingContextOwner
	{
		object DataContext { get; set; }

		void DoBind ();
		
	}

//	public abstract class BindableElement
//		: IBindableElement
//	{
//		object DataContext { get; set; }
//
//		public virtual void DoBind (){
//		}
//
//	}
}
