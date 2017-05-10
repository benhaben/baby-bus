using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyBus.Models;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Converters;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;

namespace BabyBus.ViewModels.Communication {
    public class SelectChildrenViewModel : BaseViewModel {
        private int classId;
        private readonly IMvxMessenger messenger;

        public SelectChildrenViewModel() {
            messenger = Mvx.Resolve<IMvxMessenger>();
        }

        public void Init(int id) {
            classId = id;
        }

        public override void Start() {
            base.Start();

            if (Children == null || Children.Count == 0) {
                Children = new List<ChildModel>() {
                    new ChildModel { ChildName = "王大锤", IsSelect = false },
                    new ChildModel { ChildName = "王二锤", IsSelect = false },
                    new ChildModel { ChildName = "王小锤", IsSelect = false },
                    new ChildModel { ChildName = "王美丽", IsSelect = false },
                    new ChildModel { ChildName = "张美丽", IsSelect = false },
                    new ChildModel { ChildName = "张大丽", IsSelect = false },
                    new ChildModel { ChildName = "张二丽", IsSelect = false },
                    new ChildModel { ChildName = "张三丽", IsSelect = false },
                    new ChildModel { ChildName = "张四丽", IsSelect = false },
                    new ChildModel { ChildName = "张五丽", IsSelect = false },
                };
            }
        }

        #region Property

        private List<ChildModel> children;

        public List<ChildModel> Children {
            get { return children; }
            set { 
                children = value;
                RaisePropertyChanged(() => Children);
            }
        }

        //        private ChildModel selectedChild;
        //
        //        public ChildModel SelectedChild {
        //            get { return selectedChild; }
        //            set {
        //                selectedChild = value;
        //
        //            }
        //        }

        #endregion

        private MvxCommand _selectAllChildrenCommand;

        public MvxCommand SelecetAllChildrenCommand {
            get {
                _selectAllChildrenCommand = _selectAllChildrenCommand ?? new MvxCommand(() => {
                    foreach (var child in Children) {
                        child.IsSelect = true;
                    }
                    RaisePropertyChanged(() => Children);
                });
                return _selectAllChildrenCommand;
            }
        }

        private MvxCommand _selectNoneChildrenCommand;

        public MvxCommand SelecetNoneChildrenCommand {
            get {
                _selectNoneChildrenCommand = _selectNoneChildrenCommand ?? new MvxCommand(() => {
                    foreach (var child in Children) {
                        child.IsSelect = false;
                    }
                    RaisePropertyChanged(() => Children);
                });
                return _selectNoneChildrenCommand;
            }
        }

        public IMvxCommand ReturnCommand {
            get {
                return new MvxCommand(() => {
                    List<ChildModel> list = Children.Where(x => x.IsSelect).ToList();
                    if (list == null || !list.Any()) {
                        ViewModelStatus = new ViewModelStatus("至少选择一个孩子");
                        return;
                    }
                    messenger.Publish(new ChildrenMessage(this, Children));
                    Close(this);
                });
            }
        }
    }

    public class ChildrenMessage : MvxMessage {
        public ChildrenMessage(object sender, List<ChildModel> children)
            : base(sender) {
            Children = children;
        }

        public List<ChildModel> Children { get; private set; }
    }
}
