using System;
using Cirrious.CrossCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BabyBus.Logic.Shared
{
	public class ForumMainViewModel :BaseViewModel
	{
		IRemoteService _service = null;

		public ForumIndexViewModel ForumIndexViewModel1{ get; set; }

		public ForumIndexViewModel ForumIndexViewModel2{ get; set; }

		public TeacherHomeViewModel TeacherHomeViewModel { get; private set; }

		public ParentHomeViewModel ParentHomeViewModel { get; private set; }

		public List<ForumIndexViewModel> ForumIndexViewModels{ get; set; }

		public List<ECCategory> CategoryList{ get; set; }

		public ForumMainViewModel()
		{
			_service = Mvx.Resolve<IRemoteService>();

			ForumIndexViewModel1 = new ForumIndexViewModel();
			ForumIndexViewModel2 = new ForumIndexViewModel();
			ForumIndexViewModels = new List<ForumIndexViewModel>();

			try {
				ViewModelStatus = new ViewModelStatus(UIConstants.LOADING, true, MessageType.Information, TipsType.DialogProgress);

				Task task = Task.Factory.StartNew(() => {
					CategoryList = _service.GetECCategoryList(ECColumnType.Forum).Result;
				});
				task.Wait();
				ViewModelStatus = new ViewModelStatus(UIConstants.LOAD_SUCCESS, false, MessageType.Success, TipsType.Undisplay);

				foreach (var category in CategoryList) {
					ForumIndexViewModels.Add(new ForumIndexViewModel(category.Id));
				}


			} catch (Exception ex) {
				ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Error, TipsType.DialogDisappearAuto);
			}
		}

	}
}

