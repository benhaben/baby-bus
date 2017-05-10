using CoolBeans.Pages.ChildEx;
using CoolBeans.Pages.GrowMemory;
using CoolBeans.Pages.Main;
using CoolBeans.Pages.PhysiqueHealth;
using CoolBeans.Pages.Communication;

using CoolBeans.Services.ChildEx;
using CoolBeans.Services.GrowMemory;
using CoolBeans.Services.Main;
using CoolBeans.Services.PhysiqueHealth;
using CoolBeans.Services.Communication;

using CoolBeans.ViewModels.ChildEx;
using CoolBeans.ViewModels.GrowMemory;
using CoolBeans.ViewModels.Main;
using CoolBeans.ViewModels.PhysiqueHealth;
using CoolBeans.ViewModels.Communication;
using Xamarin.Forms;
using System.Threading;

using System.Threading.Tasks;

namespace CoolBeans.Pages
{
    public class MainTabbedPage : TabbedPage
    {
        public RegisterDetailPage MyRegisterDetailPage { get; set; }

        public LoginPage MyLoginPage { get; set; }

        public GrowMemoryPage GrowMemoryPage { get; set; }

        public MyPhysiquePage MyPhysiquePage { get; set; }

        public ChildExMainPage ChildExMainPage { get; set; }

        public MainPrtPage MainPrtPage { get; set; }

        public CommTabPage CommTabPage { get; set; }

        



        public MainTabbedPage()
        {
            //CurrentPageChanged += (sender, e) => {

            //};
            //MyRegisterDetailPage = new RegisterDetailPage();
            //MyLoginPage = new LoginPage();

            //主页面
            MainPrtPage = new MainPrtPage();
            MainPrtViewModel mainPrtViewModel = new MainPrtViewModel(new MainService());
            mainPrtViewModel.Start();
            MainPrtPage.BindingContext = mainPrtViewModel;
            MainPrtPage.Title = "主页面";

            //体质与健康
//            MyPhysiquePage = new MyPhysiquePage();
//            MyPhysiqueViewModel myPhysiqueViewModel = new MyPhysiqueViewModel(new MyPhysiqueService());
//            myPhysiqueViewModel.Init();
//            myPhysiqueViewModel.Start();
//            MyPhysiquePage.BindingContext = myPhysiqueViewModel;
//            MyPhysiquePage.Title = "体质与健康";

            //家园通
            CommTabPage = new CommTabPage();
            //MyPhysiqueViewModel myPhysiqueViewModel = new MyPhysiqueViewModel(new MyPhysiqueService());
            //myPhysiqueViewModel.Init();
            //myPhysiqueViewModel.Start();
            //MyPhysiquePage.BindingContext = myPhysiqueViewModel;
            CommTabPage.Title = "家园通";


			//成长记忆
            GrowMemoryPage = new GrowMemoryPage(); 
            GrowMemoryPage.Title = "成长记忆";
			LoadGrowMemoryPage();
            //new Task(LoadGrowMemoryPage).Start();

            //幼教经验
            ChildExMainPage = new ChildExMainPage();
            ChildExMainPage.Title = "幼教经验";
			LoadChildExPage ();
            //new Task(LoadChildExPage).Start();

            



            //this.Children.Add(MyRegisterDetailPage);
            //this.Children.Add(MyLoginPage);
            this.Children.Add(MainPrtPage);
            this.Children.Add(CommTabPage);
            this.Children.Add(GrowMemoryPage);
            this.Children.Add(ChildExMainPage);
            //this.Children.Add(MyPhysiquePage);
            this.SetValue(TitleProperty, "BabyBus");
        }

		private void LoadGrowMemoryPage()
		{

				GrowMemoryViewModel growMemoryViewModel = new GrowMemoryViewModel (new GrowMemoryService ());
				growMemoryViewModel.Start ();
				GrowMemoryPage.BindingContext = growMemoryViewModel;

		}

		private void LoadChildExPage()
		{

				ChildExMainViewModel childExMainViewModel = new ChildExMainViewModel (new ChildExService ());
				childExMainViewModel.Start ();
				ChildExMainPage.BindingContext = childExMainViewModel;

		}

    }
}
