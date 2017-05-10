using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using CoolBeans.ViewModels.Communication;
using CoolBeans.Services.Communication;

namespace CoolBeans.Pages.Communication
{
    public class CommTabPage : CarouselPage
    {
        public ClassArtListViewModel classArtListViewModel { get; set; }
        public KindergartenArtListViewModel kindergartenArtListViewModel { get; set; }

        public ClassArtListPage _classArtList { get; set; }
        public KindergartenArtListPage _kindergartenArtList { get; set; }
        //public CommListPage _commListPage { get; set; }


        public CommTabPage()
        {
            _classArtList = new ClassArtListPage();
            _kindergartenArtList = new KindergartenArtListPage();
            //_commListPage = new CommListPage();

            classArtListViewModel = new ClassArtListViewModel(new ClassALlistService());
            kindergartenArtListViewModel = new KindergartenArtListViewModel(new KindergartenAListService());
            //CommListViewModel commListViewModel = new CommListViewModel(new CommService());

            classArtListViewModel.Init();
            kindergartenArtListViewModel.Init();

            classArtListViewModel.Start();
            kindergartenArtListViewModel.Start();
            //commListViewModel.Start();

            

            _classArtList.BindingContext = classArtListViewModel;
            _kindergartenArtList.BindingContext = kindergartenArtListViewModel;
            //_commListPage.BindingContext = commListViewModel;

            this.Children.Add(_classArtList);
            this.Children.Add(_kindergartenArtList);
            //this.Children.Add(_commListPage);
            
            this.SetValue(TitleProperty, "家园通");
        }
    }
}
