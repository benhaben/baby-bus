using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BabyBus.Logic.Shared;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using Newtonsoft.Json;


namespace BabyBus.Logic.Shared
{
	public class SelectChildrenViewModel : BaseViewModel
	{

		private long _classId = BabyBusContext.ClassId;
		private readonly IRemoteService _remoteService;
		private readonly IMvxMessenger _messenger;

		public SelectChildrenViewModel(IMvxMessenger messenger)
		{
			_remoteService = Cirrious.CrossCore.Mvx.Resolve<IRemoteService>();
			_messenger = messenger;
			#if DEBUG1
			Children.Add(new ChildModel("yin"));
			Children.Add(new ChildModel("shen"));
			Children.Add(new ChildModel("shen1"));
			Children.Add(new ChildModel("shen2"));
			Children.Add(new ChildModel("shen3"));
			Children.Add(new ChildModel("shen4"));
			Children.Add(new ChildModel("shen5"));
			Children.Add(new ChildModel("shen6"));
			Children.Add(new ChildModel("shen7"));
			Children.Add(new ChildModel("shen8"));
			Children.Add(new ChildModel("shen8"));
			Children.Add(new ChildModel("shen8"));
			Children.Add(new ChildModel("shen8"));
			Children.Add(new ChildModel("shen8"));
			Children.Add(new ChildModel("shen8"));
			Children.Add(new ChildModel("shen8"));
			Children.Add(new ChildModel("shen8"));
			Children.Add(new ChildModel("shen8"));
			Children.Add(new ChildModel("shen8"));
			Children.Add(new ChildModel("shen8"));
			Children.Add(new ChildModel("shen8"));
			Children.Add(new ChildModel("shen8"));
			Children.Add(new ChildModel("shen8"));
			Children.Add(new ChildModel("shen8"));
			Children.Add(new ChildModel("shen8"));
			Children.Add(new ChildModel("shen8"));


			#endif
		}

		public override void InitData()
		{
			base.InitData();
			try {
				ViewModelStatus = new ViewModelStatus("正在加载孩子名单...", true);
				Children = _remoteService.GetChildren().Result;

				if (_childrenSelected != null && Children != null) {
					foreach (var selected in _childrenSelected) {
						foreach (var raw in Children) {
							if (raw.ChildId == selected.ChildId) {
								raw.IsSelect = selected.IsSelect;
							}
						}
					}
				}
				ViewModelStatus = new ViewModelStatus("加载成功", false, MessageType.Success, TipsType.Undisplay);

			} catch (Exception ex) {
				ViewModelStatus = new ViewModelStatus(ex.Message, false, MessageType.Error);

			}
		}

		List<ChildModel> _childrenSelected;

		public void Init(string jsonString)
		{ 
			_childrenSelected = JsonConvert.DeserializeObject<List<ChildModel>>(jsonString);
		}

		#region Property

		private List<ChildModel> children = new List<ChildModel>();

		public List<ChildModel> Children {
			get { return children; }
			set {
				children = value;
			}
		}

		public int Total {
			get { return Children.Count; }
		}

		public int Attence {
			get { return Children.Count(x => x.IsSelect); }
		}

		#endregion

		private IMvxCommand _finishCommand;

		public IMvxCommand FinishCommand {
			get {
				_finishCommand = _finishCommand ?? new MvxCommand(() => {
					var list = Children.Where(x => x.IsSelect).ToList();
					if (list.Count == 0) {
						ViewModelStatus = new ViewModelStatus("请至少选择一个孩子");
					} else {
						var message = new ChildrenMessage(this, list);
						_messenger.Publish(message);
						Close(this);
					}
				});
				return _finishCommand;
			}
		}
	}
}
