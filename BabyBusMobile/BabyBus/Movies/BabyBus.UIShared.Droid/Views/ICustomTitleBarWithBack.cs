using System;

namespace BabyBus.Droid.Views
{
	public interface ICustomTitleBarWithBack
	{
		void SetCustomTitleWithBack(int layoutId,int titleId);
		void SetCustomTitleWithBack(int layoutId,string title);
	}
}

