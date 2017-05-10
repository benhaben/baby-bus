using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyBus.Helpers;
using BabyBus.Models.Communication;
using BabyBus.Utilities;
using Cirrious.MvvmCross.ViewModels;

namespace BabyBus.ViewModels.Communication {
    public class NoticeDetailViewModel : BaseViewModel {
        private int _id;

        public NoticeDetailViewModel() {
            
        }

        public NoticeDetailViewModel(NoticeModel notice) {
            Notice = notice;
        }

        public void Init(int id) {
            _id = id;
        }

        public override void Start() {
            base.Start();

            var notices = BabyBusContext.BaseNoticeList;
            Notice = notices.FirstOrDefault(x => x.Id == _id);
        }

        private NoticeModel notice;

        public NoticeModel Notice {
            get { return notice; }
            set {
                notice = value;
                RaisePropertyChanged(() => Notice);
            }
        }

        //Note : don't moved this to parent, Android Adapter use it.
        private IMvxCommand showDetailCommand;

        public IMvxCommand ShowDetailCommand {
            get {
                showDetailCommand = showDetailCommand ?? new MvxCommand(() => {
                    ShowViewModel<NoticeDetailViewModel>(new {id = Notice.Id});
                });
                return showDetailCommand;
            }
        }
    }
}
