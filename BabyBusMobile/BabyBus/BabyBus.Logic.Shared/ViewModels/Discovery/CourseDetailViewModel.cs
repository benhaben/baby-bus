using System;
using Cirrious.MvvmCross.ViewModels;

namespace BabyBus.Logic.Shared
{
	public class CourseDetailViewModel : BaseViewModel
	{
		int _postinfoId;

		public CourseDetailViewModel()
		{
		}

		IMvxCommand _showReviewListCommand;

		public IMvxCommand ShowReviewListCommand {
			get {
				_showReviewListCommand = _showReviewListCommand
				?? new MvxCommand(() => 
						ShowViewModel<CourseReviewListViewModel>(new {id = _postinfoId}));
				return _showReviewListCommand;
			}
		}
	}
}

