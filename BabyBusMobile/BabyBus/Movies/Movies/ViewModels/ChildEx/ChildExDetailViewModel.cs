using BabyBus.Services.ChildEx;
using Cirrious.MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyBus.ViewModels.ChildEx
{
    public class ChildExDetailViewModel : MvxViewModel
    {
        private Models.ChildEx.ChildEx _childEx;
        private int _id;

        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime InDate { get; set; }

        public Models.ChildEx.ChildEx ChildEx
        {
            get { return _childEx; }
        }

        public void Init(int id)
        {
            _id = id;
        }

        public override async void Start()
        {
            base.Start();

            if (_childEx != null)
                return;
            _childEx = await _service.GetById(_id);
            Title = _childEx.Title;
            Content = _childEx.Content;
            InDate = _childEx.InDate;
        }

        private IChildExService _service;

        public ChildExDetailViewModel(IChildExService service)
        {
            _service = service;
        }
    }
}
