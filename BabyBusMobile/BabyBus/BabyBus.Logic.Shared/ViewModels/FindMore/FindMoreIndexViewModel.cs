using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Cirrious.CrossCore;

namespace BabyBus.Logic.Shared
{
	public class FindMoreIndexViewModel:BaseListViewModel
	{

		private IRemoteService _service;
		bool isFirst = true;

		public FindMoreIndexViewModel()
		{
			_service = Mvx.Resolve<IRemoteService>();
		}

		public List<AdvertisementModel> AdvertisementModel{ get; set; }

		List<string> _advertisementStrList;

		public List<string> AdvertisementStrList {
			get {
				if (_advertisementStrList == null) {
					_advertisementStrList = new List<string>();
					if (AdvertisementModel == null) {
						AdvertisementModel = new List<AdvertisementModel>();
					}
					AdvertisementModel.ForEach(x => _advertisementStrList.Add(x.NormalPics));
				}
				return _advertisementStrList;
			}
		}

		public override void InitData()
		{
			
			base.InitData();
			if (isFirst) {
				ViewModelStatus = new ViewModelStatus(UIConstants.LOADING, true, MessageType.Information, TipsType.DialogProgress);
			}
			isFirst = false;
			try {
				var task = Task.Factory.StartNew(() => {
					AdvertisementModel = _service.GetAdvertisement().Result;
					ListObject = _service.GetRecommendPosts().Result;
					ViewModelStatus = new ViewModelStatus(UIConstants.LOAD_SUCCESS, false, MessageType.Success, TipsType.Undisplay);
				});
				task.Wait();

			} catch (Exception ex) {
				Mvx.Trace(ex.Message);
			}
		}

		public void ShowRecommendationDetailViewModel(int id, ECColumnType ColumnType)
		{
			ShowViewModel<CourseDetailViewModel>(new {postinfoId = id,eCColumnType = ColumnType});
		}

		public void ShowActivityCourseIndexViewModel(ECColumnType ec)
		{
			ShowViewModel<ActivityCourseIndexViewModel>(new {eCCoulumType = ec});
		}

		public void ShowForumMainViewModel()
		{
			ShowViewModel<ForumIndexViewModel>();
		}

		public void ShowForumIndexViewModel()
		{
			ShowViewModel<ForumIndexViewModel>();
		}
	}
}

