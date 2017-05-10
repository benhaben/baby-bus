using BabyBus.Logic.Shared;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using UIKit;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Touch.Platform;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.CrossCore;



namespace BabyBus.iOS
{
    public class Setup : MvxTouchSetup
    {
        private MvxApplicationDelegate _applicationDelegate;
        private UIWindow _window;

        public Setup(MvxApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
            _applicationDelegate = applicationDelegate;
            _window = window;
        }

        protected override IMvxTouchViewPresenter CreatePresenter()
        {
            return new MyPresenter(_applicationDelegate, _window);
//            return new MvxFormsTouchViewPresenter(Window);
        }

        protected override IMvxApplication CreateApp()
        {
            Mvx.LazyConstructAndRegisterSingleton<IPictureService, PictureService>();
            Mvx.LazyConstructAndRegisterSingleton<IEnvironmentService, EnvironmentService>();
            #if __PARENT__
            BaseViewModel.AppType = AppType.Parent;
            #elif __TEACHER__
            BaseViewModel.AppType = AppType.Teacher;
            #elif __MASTER__
            BaseViewModel.AppType = AppType.Master;
            #endif

            return new App(true);
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

        protected override void FillBindingNames(Cirrious.MvvmCross.Binding.BindingContext.IMvxBindingNameRegistry registry)
        {
            // use these to register default binding names
            //registry.AddOrOverwrite<NicerBinaryEdit>(be => be.MyCount);
            //registry.AddOrOverwrite(typeof(BinaryEdit),"N28Doofus");
            base.FillBindingNames(registry);
        }

        protected override void FillTargetFactories(Cirrious.MvvmCross.Binding.Bindings.Target.Construction.IMvxTargetBindingFactoryRegistry registry)
        {
            //TODO : use key－value instead
//            registry.RegisterFactory(new MvxSimplePropertyInfoTargetBindingFactory(typeof(MvxRadioRootElementEnhanceBinding<CityModel>),
//                    typeof(RadioRootElement<CityModel>), "EnhanceRadioSelected"));
//            registry.RegisterFactory(new MvxSimplePropertyInfoTargetBindingFactory(typeof(MvxRadioRootElementEnhanceBinding<KindergartenClassModel>),
//                    typeof(RadioRootElement<KindergartenClassModel>), "EnhanceRadioSelected"));
//            registry.RegisterFactory(new MvxSimplePropertyInfoTargetBindingFactory(typeof(MvxRadioRootElementEnhanceBinding<KindergartenModel>),
//                    typeof(RadioRootElement<KindergartenModel>), "EnhanceRadioSelected"));
            registry.RegisterFactory(new MvxSimplePropertyInfoTargetBindingFactory(typeof(MvxRadioRootElementEnhanceBinding<GenderModel>),
                    typeof(RadioRootElement<GenderModel>), "EnhanceRadioSelected"));
//            registry.RegisterCustomBindingFactory<BindableSection<ChildViewElement> >(
//                "ItemsSource",
//                              binary => new BinaryEditFooTargetBinding(binary));

//			registry.RegisterCustomBindingFactory<BinaryEdit>(
//				"N28",
//				binary => new BinaryEditFooTargetBinding(binary));
            base.FillTargetFactories(registry);
        }
    }

    public class MyPresenter : MvxTouchViewPresenter
    {
        public MyPresenter(UIApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
        }

        protected override UINavigationController CreateNavigationController(UIViewController viewController)
        {
            var navBar = base.CreateNavigationController(viewController);
            navBar.NavigationBarHidden = true;

            return navBar;
        }

        //		private ParentHomeView _firstView;
        //
        //		public override void Show(Cirrious.MvvmCross.Touch.Views.IMvxTouchView view)
        //		{
        //			if (view is ParentHomeView)
        //			{
        //				_firstView = view as ParentHomeView;
        //			}
        //
        //			if (view is GrandChildView)
        //			{
        //				if (_firstView != null)
        //				{
        //					_firstView.ShowGrandChild(view);
        //				}
        //				return;
        //			}
        //
        //			base.Show(view);
        //		}
    }
}