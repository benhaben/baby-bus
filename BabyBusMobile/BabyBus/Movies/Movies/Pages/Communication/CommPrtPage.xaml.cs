using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoolBeans.Net.Communication;
using CoolBeans.ViewModels.Communication;
using Xamarin.Forms;

namespace CoolBeans.Pages.Communication
{
    public partial class CommPrtPage : CarouselPage
    {
        public CommArticleListPage KgCommArticleListPage { get; set; }
        public CommArticleListPage ClassArticleListPage { get; set; }
        public PrtQuestionListPage PrtQuestionListPage { get; set; }
        public CommPrtPage()
        {
            InitializeComponent();

            //园区通知
            KgCommArticleListPage = new CommArticleListPage();
            var kgViewModel = new CommArticleListViewModel(new ArticleService());
            kgViewModel.Init(1);
            kgViewModel.Start();
            KgCommArticleListPage.BindingContext = kgViewModel;
            KgCommArticleListPage.Title = "园区通知";

            //班级通知
            ClassArticleListPage = new CommArticleListPage();
            var classViewModel = new CommArticleListViewModel(new ArticleService());
            classViewModel.Init(2);
            classViewModel.Start();
			ClassArticleListPage.BindingContext = classViewModel;
			ClassArticleListPage.Title = "班级通知";

            //家园联系
            PrtQuestionListPage = new PrtQuestionListPage();
            var prtQuestionListViewModel = new PrtQuestionListViewModel(new QuestionService());
            prtQuestionListViewModel.Start();
            PrtQuestionListPage.BindingContext = prtQuestionListViewModel;
            PrtQuestionListPage.Title = "家园联系";

            Task.WhenAll();

            this.Children.Add(KgCommArticleListPage);
            this.Children.Add(ClassArticleListPage);
            this.Children.Add(PrtQuestionListPage);
        }
    }
}
