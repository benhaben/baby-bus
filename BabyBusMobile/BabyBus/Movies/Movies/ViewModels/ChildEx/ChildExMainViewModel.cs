using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using BabyBus.Models.ChildEx;
using BabyBus.Services.ChildEx;
using Cirrious.MvvmCross.ViewModels;

namespace BabyBus.ViewModels.ChildEx
{
    public class ChildExMainViewModel : MvxViewModel
    {
        private readonly IChildExService _service;

        private List<Models.ChildEx.ChildEx> _childExs;

        private ObservableCollection<ChildExList> _childExsCollection;

        private Models.ChildEx.ChildEx _selectedChildEx;

        public ChildExMainViewModel(IChildExService service)
        {
            _service = service;
        }

        public List<Models.ChildEx.ChildEx> ChildExs
        {
            get { return _childExs; }
            set
            {
                _childExs = value;
                RaisePropertyChanged(() => ChildExs);

                //Group experience by type
                _childExsCollection = new ObservableCollection<ChildExList>();

                foreach (Models.ChildEx.ChildEx item in ChildExs)
                {
                    ChildExList clct = _childExsCollection.FirstOrDefault(g => g.ChildExType == item.ChildExType);
                    if (clct == null)
                    {
                        clct = new ChildExList();
                        clct.Title = item.ChildExType.ToString();
                        clct.ChildExType = item.ChildExType;
						clct.SlideDisplay = item.SlideDisplay;
                        clct.ChildExs = new List<Models.ChildEx.ChildEx>();
                        clct.ChildExs.Add(item);
                        _childExsCollection.Add(clct);
                    }
                    else
                    {
                        clct.ChildExs.Add(item);
                    }
                }
            }
        }

        public ObservableCollection<ChildExList> ChildExsCollection
        {
            get { return _childExsCollection; }
        }

        public Models.ChildEx.ChildEx SelectedChildEx
        {
            get { return _selectedChildEx; }
            set
            {
                _selectedChildEx = value;
                RaisePropertyChanged(() => SelectedChildEx);
            }
        }

        public ICommand ShowSelectedChildExCommand
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<ChildExDetailViewModel>(new {id = SelectedChildEx.Id})
                    , () => SelectedChildEx != null);

            }
        }

        public override async void Start()
        {
            base.Start();

            ChildExs = await _service.SearchChildEx();
        }
    }

    public class ChildExList : MvxViewModel
    {
        public string Title { get; set; }

		public string SlideDisplay { get; set; }

        public ChildExType ChildExType { get; set; }

        public List<Models.ChildEx.ChildEx> ChildExs { get; set; }

        private Models.ChildEx.ChildEx _selectedChildEx;

        public Models.ChildEx.ChildEx SelectedChildEx
        {
            get { return _selectedChildEx; }
            set
            {
                _selectedChildEx = value;
                RaisePropertyChanged(()=>SelectedChildEx);
                ShowSelectedChildExCommand.Execute(null);

            }
        }

        public ICommand ShowSelectedChildExCommand
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<ChildExDetailViewModel>(new { id = SelectedChildEx.Id })
                    , () => SelectedChildEx != null);
            }
        }
    }
}